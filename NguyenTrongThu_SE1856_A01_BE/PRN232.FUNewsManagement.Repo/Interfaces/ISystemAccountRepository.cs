using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface ISystemAccountRepository : IBaseRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email, short? excludeAccountId = null);
        Task<bool> HasCreatedNewsArticlesAsync(short accountId);
        Task<(IEnumerable<SystemAccount> items, int totalCount)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? searchTerm = null,
            int? role = null,
            string? sortBy = null,
            bool isDescending = false);
    }
}
