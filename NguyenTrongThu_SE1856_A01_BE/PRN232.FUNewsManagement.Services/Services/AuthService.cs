using Microsoft.Extensions.Configuration;
using PRN232.FUNewsManagement.Models.Enums;
using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Auth;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Helpers;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            // Check admin account from appsettings
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                var token = _tokenService.GenerateToken(0, adminEmail!, 0); // Role 0 for Admin
                return new LoginResponse
                {
                    Token = token,
                    Email = adminEmail!,
                    Role = 0,
                    RoleName = "Admin"
                };
            }

            // Check regular account
            var account = await _unitOfWork.SystemAccounts.GetByEmailAsync(request.Email);
            if (account == null)
            {
                return null;
            }

            if (!PasswordHelper.VerifyPassword(request.Password, account.AccountPassword))
            {
                return null;
            }

            var userToken = _tokenService.GenerateToken(
                account.AccountID,
                account.AccountEmail,
                account.AccountRole);

            return new LoginResponse
            {
                Token = userToken,
                AccountId = account.AccountID,
                Email = account.AccountEmail,
                Name = account.AccountName,
                Role = account.AccountRole,
                RoleName = ((AccountRole)account.AccountRole).ToString()
            };
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            return await Task.FromResult(_tokenService.ValidateToken(token));
        }
    }
}
