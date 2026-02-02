using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Services;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
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
                return BadRequest(ModelState);
            }

            // Check if it's admin login
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                var token = _jwtHelper.GenerateAdminToken(adminEmail!, "Administrator");
                return Ok(new LoginResponse
                {
                    AccountId = 0,
                    AccountName = "Administrator",
                    AccountEmail = adminEmail!,
                    AccountRole = 0, // Admin role
                    Token = token
                });
            }

            // Check regular user authentication
            var account = _accountService.AuthenticateUser(request.Email, request.Password);
            if (account == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var userToken = _jwtHelper.GenerateToken(account);
            return Ok(new LoginResponse
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName!,
                AccountEmail = account.AccountEmail!,
                AccountRole = account.AccountRole,
                Token = userToken
            });
        }
    }
}
