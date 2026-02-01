using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.UnitOfWork;
using PRN232.FUNewsManagementSystem.Services.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN232.FUNewsManagementSystem.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<(SystemAccountDto account, string token)?> LoginAsync(string email, string password)
        {
            var account = await _unitOfWork.SystemAccountRepository.GetByEmailAndPasswordAsync(email, password);

            if (account == null)
                return null;

            var accountDto = new SystemAccountDto
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole
            };

            var token = GenerateJwtToken(accountDto);

            return (accountDto, token);
        }

        public async Task<SystemAccountDto> RegisterAsync(SystemAccountDto accountDto, string password)
        {
            if (await _unitOfWork.SystemAccountRepository.IsEmailExistAsync(accountDto.AccountEmail))
                throw new Exception("Email already exists");

            var account = new SystemAccount
            {
                AccountName = accountDto.AccountName,
                AccountEmail = accountDto.AccountEmail,
                AccountPassword = password, // In production, hash this!
                AccountRole = accountDto.AccountRole
            };

            await _unitOfWork.SystemAccountRepository.CreateAsync(account);

            accountDto.AccountId = account.AccountId;
            return accountDto;
        }

        public string GenerateJwtToken(SystemAccountDto account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.AccountEmail),
                new Claim(ClaimTypes.Name, account.AccountName),
                new Claim(ClaimTypes.Role, account.AccountRole?.ToString() ?? "2"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}