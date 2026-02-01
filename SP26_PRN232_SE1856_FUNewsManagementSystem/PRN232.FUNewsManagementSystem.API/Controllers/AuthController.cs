using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.API.Models.Common;
using PRN232.FUNewsManagementSystem.API.Models.Request;
using PRN232.FUNewsManagementSystem.API.Models.Response;
using PRN232.FUNewsManagementSystem.Services.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<LoginResponse>.ErrorResponse("Validation failed", errors));
            }

            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result == null)
            {
                return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid email or password"));
            }

            var response = new LoginResponse
            {
                Token = result.Value.token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new SystemAccountResponse
                {
                    AccountId = result.Value.account.AccountId,
                    AccountName = result.Value.account.AccountName,
                    AccountEmail = result.Value.account.AccountEmail,
                    AccountRole = result.Value.account.AccountRole
                }
            };

            return Ok(ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful"));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<SystemAccountResponse>>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<SystemAccountResponse>.ErrorResponse("Validation failed", errors));
            }

            try
            {
                var accountDto = new SystemAccountDto
                {
                    AccountName = request.AccountName,
                    AccountEmail = request.AccountEmail,
                    AccountRole = request.AccountRole
                };

                var result = await _authService.RegisterAsync(accountDto, request.AccountPassword);

                var response = new SystemAccountResponse
                {
                    AccountId = result.AccountId,
                    AccountName = result.AccountName,
                    AccountEmail = result.AccountEmail,
                    AccountRole = result.AccountRole
                };

                return CreatedAtAction(nameof(Register), ApiResponse<SystemAccountResponse>.SuccessResponse(response, "Registration successful"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<SystemAccountResponse>.ErrorResponse(ex.Message));
            }
        }
    }
}