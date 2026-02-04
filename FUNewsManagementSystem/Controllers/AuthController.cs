using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using FUNewsManagementSystem.Models.Common;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISystemAccountService _accountService;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        public AuthController(ISystemAccountService accountService, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { errors });
            }

            // Check if it's admin login
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                var token = _jwtHelper.GenerateAdminToken(adminEmail!, "Administrator");
                var loginResponse = new LoginResponse
                {
                    AccountId = 0,
                    AccountName = "Administrator",
                    AccountEmail = adminEmail!,
                    AccountRole = 0, // Admin role
                    Token = token
                };
                
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(
                    loginResponse,
                    "Login successful"));
            }

            // Check regular user authentication
            var account = _accountService.AuthenticateUser(request.Email, request.Password);
            if (account == null)
            {
                return Unauthorized(new { 
                    errors = new { 
                        authentication = new[] { "Invalid email or password" } 
                    } 
                });
            }

            var userToken = _jwtHelper.GenerateToken(account);
            var userLoginResponse = new LoginResponse
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName!,
                AccountEmail = account.AccountEmail!,
                AccountRole = account.AccountRole,
                Token = userToken
            };
            
            return Ok(ApiResponse<LoginResponse>.SuccessResponse(
                userLoginResponse,
                "Login successful"));
        }
    }
}
