using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount> GetByEmailAsync(string email);
        Task<SystemAccount> GetByEmailAndPasswordAsync(string email, string password);
        Task<List<SystemAccount>> GetAccountsByRoleAsync(int role);
        Task<bool> IsEmailExistAsync(string email);
    }
}
