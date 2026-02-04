using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FUNewsManagementSystem.Models.Common;

namespace FUNewsManagementSystem.Filters
{
    /// <summary>
    /// Action filter that adds Link headers for pagination following Shopify API standards
    /// Automatically detects PaginatedResponse and generates prev/next/first/last links
    /// </summary>
    public class LinkHeaderPaginationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No action needed before execution
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is not OkObjectResult okResult)
                return;

            // Check if response contains ApiResponse<PaginatedResponse<T>>
            var responseType = okResult.Value?.GetType();
            if (responseType == null || !responseType.IsGenericType)
                return;

            var genericTypeDef = responseType.GetGenericTypeDefinition();
            if (genericTypeDef != typeof(ApiResponse<>))
                return;

            var dataProperty = responseType.GetProperty("Data");
            var data = dataProperty?.GetValue(okResult.Value);
            if (data == null)
                return;

            var dataType = data.GetType();
            if (!dataType.IsGenericType || dataType.GetGenericTypeDefinition() != typeof(PaginatedResponse<>))
                return;

            // Extract pagination info
            var pageProperty = dataType.GetProperty("Page");
            var pageSizeProperty = dataType.GetProperty("PageSize");
            var totalPagesProperty = dataType.GetProperty("TotalPages");

            var currentPage = (int)(pageProperty?.GetValue(data) ?? 1);
            var pageSize = (int)(pageSizeProperty?.GetValue(data) ?? 10);
            var totalPages = (int)(totalPagesProperty?.GetValue(data) ?? 1);

            // Build base URL
            var request = context.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}";
            var queryParams = request.Query
                .Where(q => q.Key.ToLower() != "page")
                .Select(q => $"{q.Key}={q.Value}")
                .ToList();

            var linkHeaders = new List<string>();

            // Add "next" link
            if (currentPage < totalPages)
            {
                var nextParams = new List<string>(queryParams) { $"page={currentPage + 1}", $"pageSize={pageSize}" };
                linkHeaders.Add($"<{baseUrl}?{string.Join("&", nextParams)}>; rel=\"next\"");
            }

            // Add "previous" link
            if (currentPage > 1)
            {
                var prevParams = new List<string>(queryParams) { $"page={currentPage - 1}", $"pageSize={pageSize}" };
                linkHeaders.Add($"<{baseUrl}?{string.Join("&", prevParams)}>; rel=\"previous\"");
            }

            // Add "first" link
            var firstParams = new List<string>(queryParams) { "page=1", $"pageSize={pageSize}" };
            linkHeaders.Add($"<{baseUrl}?{string.Join("&", firstParams)}>; rel=\"first\"");

            // Add "last" link
            var lastParams = new List<string>(queryParams) { $"page={totalPages}", $"pageSize={pageSize}" };
            linkHeaders.Add($"<{baseUrl}?{string.Join("&", lastParams)}>; rel=\"last\"");

            // Set Link header
            if (linkHeaders.Any())
            {
                context.HttpContext.Response.Headers["Link"] = string.Join(", ", linkHeaders);
            }
        }
    }
}
