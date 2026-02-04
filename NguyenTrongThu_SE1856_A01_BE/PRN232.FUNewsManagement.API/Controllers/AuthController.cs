using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login to the system
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var result = await _authService.LoginAsync(request);
            if (result == null)
            {
                return Unauthorized(ApiResponse<object>.FailureResult("Invalid email or password"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Login successful"));
        }
    }
}
