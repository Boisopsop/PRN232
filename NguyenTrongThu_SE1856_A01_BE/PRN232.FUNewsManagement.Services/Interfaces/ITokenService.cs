using System.Security.Claims;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(short accountId, string email, int role);
        bool ValidateToken(string token);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}
