using System.Collections.Concurrent;

namespace FUNewsManagementSystem.Middleware
{
    /// <summary>
    /// Rate limiting middleware following Shopify API standards
    /// Tracks requests per IP address with sliding window
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, ClientRateLimit> _clients = new();
        private readonly int _requestLimit;
        private readonly TimeSpan _timeWindow;

        public RateLimitingMiddleware(RequestDelegate next, int requestLimit = 40, int timeWindowMinutes = 1)
        {
            _next = next;
            _requestLimit = requestLimit;
            _timeWindow = TimeSpan.FromMinutes(timeWindowMinutes);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            var clientLimit = _clients.GetOrAdd(clientId, _ => new ClientRateLimit());

            bool rateLimitExceeded = false;
            int remaining = 0;
            long resetTime = 0;

            lock (clientLimit)
            {
                var now = DateTime.UtcNow;
                
                // Remove expired requests from sliding window
                clientLimit.Requests.RemoveAll(r => now - r > _timeWindow);

                // Check if limit exceeded
                if (clientLimit.Requests.Count >= _requestLimit)
                {
                    rateLimitExceeded = true;
                    var oldestRequest = clientLimit.Requests.Min();
                    var reset = oldestRequest.Add(_timeWindow);
                    resetTime = new DateTimeOffset(reset).ToUnixTimeSeconds();
                }
                else
                {
                    // Add current request
                    clientLimit.Requests.Add(now);
                    remaining = _requestLimit - clientLimit.Requests.Count;
                    
                    var nextReset = clientLimit.Requests.Min().Add(_timeWindow);
                    resetTime = new DateTimeOffset(nextReset).ToUnixTimeSeconds();
                }
            }

            // Add rate limit headers
            context.Response.Headers["X-RateLimit-Limit"] = _requestLimit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = rateLimitExceeded ? "0" : remaining.ToString();
            context.Response.Headers["X-RateLimit-Reset"] = resetTime.ToString();

            if (rateLimitExceeded)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.Response.Headers["Retry-After"] = ((int)(DateTimeOffset.FromUnixTimeSeconds(resetTime) - DateTimeOffset.UtcNow).TotalSeconds).ToString();
                
                await context.Response.WriteAsJsonAsync(new
                {
                    errors = new
                    {
                        rate_limit = new[] { "Rate limit exceeded. Please try again later." }
                    }
                });
                return;
            }

            // Cleanup old clients periodically
            if (_clients.Count > 10000)
            {
                var expiredClients = _clients
                    .Where(kvp => DateTime.UtcNow - kvp.Value.Requests.DefaultIfEmpty(DateTime.MinValue).Max() > _timeWindow.Add(TimeSpan.FromMinutes(5)))
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredClients)
                {
                    _clients.TryRemove(key, out _);
                }
            }

            await _next(context);
        }

        private string GetClientIdentifier(HttpContext context)
        {
            // Try to get real IP from proxy headers first
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                return forwardedFor.Split(',')[0].Trim();
            }

            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private class ClientRateLimit
        {
            public List<DateTime> Requests { get; set; } = new();
        }
    }

    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder, int requestLimit = 40, int timeWindowMinutes = 1)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>(requestLimit, timeWindowMinutes);
        }
    }
}
