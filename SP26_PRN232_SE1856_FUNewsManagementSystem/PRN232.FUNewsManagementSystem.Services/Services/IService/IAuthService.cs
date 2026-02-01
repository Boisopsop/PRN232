using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Services.Models;

namespace PRN232.FUNewsManagementSystem.Services.Services.IService
{
    public interface IAuthService
    {
        Task<(SystemAccountDto account, string token)?> LoginAsync(string email, string password);
        Task<SystemAccountDto> RegisterAsync(SystemAccountDto accountDto, string password);
        string GenerateJwtToken(SystemAccountDto account);
    }
}
