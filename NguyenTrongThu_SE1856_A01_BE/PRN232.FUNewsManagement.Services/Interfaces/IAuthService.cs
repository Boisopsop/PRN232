using PRN232.FUNewsManagement.Models.Request.Auth;
using PRN232.FUNewsManagement.Models.Response.Auth;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool> ValidateTokenAsync(string token);
    }
}
