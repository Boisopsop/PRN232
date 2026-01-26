using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public interface ISystemAccountRepository : IRepository<SystemAccount>
    {
        SystemAccount? GetByEmail(string email);
        SystemAccount? AuthenticateUser(string email, string password);
        bool CanDeleteAccount(short accountId);
    }

    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public SystemAccount? GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(a => a.AccountEmail == email);
        }

        public SystemAccount? AuthenticateUser(string email, string password)
        {
            return _dbSet.FirstOrDefault(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public bool CanDeleteAccount(short accountId)
        {
            // Check if account has created any news articles
            return !_context.NewsArticles.Any(n => n.CreatedById == accountId);
        }

        public override IQueryable<SystemAccount> GetAll()
        {
            return _dbSet.AsQueryable();
        }
    }
}
