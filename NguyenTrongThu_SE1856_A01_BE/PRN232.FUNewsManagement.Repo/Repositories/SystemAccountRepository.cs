using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class SystemAccountRepository : BaseRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.AccountEmail == email);
        }

        public async Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null)
        {
            return await _dbSet.AnyAsync(a => 
                a.AccountEmail == email && 
                (!excludeAccountId.HasValue || a.AccountID != excludeAccountId.Value));
        }

        public async Task<bool> HasCreatedNewsArticlesAsync(short accountId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CreatedByID == accountId);
        }

        public async Task<(IEnumerable<SystemAccount> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var query = _dbSet.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a =>
                    a.AccountName.ToLower().Contains(searchTerm) ||
                    a.AccountEmail.ToLower().Contains(searchTerm));
            }

            if (role.HasValue)
            {
                query = query.Where(a => a.AccountRole == role.Value);
            }

            // Sorting
            query = ApplySorting(query, sortBy, isDescending);

            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        private IQueryable<SystemAccount> ApplySorting(
            IQueryable<SystemAccount> query,
            string? sortBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query.OrderBy(a => a.AccountName);
            }

            query = sortBy.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(a => a.AccountName)
                    : query.OrderBy(a => a.AccountName),
                "email" => isDescending
                    ? query.OrderByDescending(a => a.AccountEmail)
                    : query.OrderBy(a => a.AccountEmail),
                "role" => isDescending
                    ? query.OrderByDescending(a => a.AccountRole)
                    : query.OrderBy(a => a.AccountRole),
                _ => query.OrderBy(a => a.AccountName)
            };

            return query;
        }
    }
}
