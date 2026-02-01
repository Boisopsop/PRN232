using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsDBContext context) : base(context)
        {
        }

        public async Task<SystemAccount> GetByEmailAsync(string email)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<SystemAccount> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public async Task<List<SystemAccount>> GetAccountsByRoleAsync(int role)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountRole == role)
                .ToListAsync();
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.SystemAccounts
                .AnyAsync(a => a.AccountEmail == email);
        }
    }
}
