using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface INewsArticleRepository : IBaseRepository<NewsArticle>
    {
        Task<NewsArticle?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByStaffAsync(short staffId);
        Task<string> GenerateNewsIdAsync();
        Task<(IEnumerable<NewsArticle> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<IEnumerable<NewsArticle>> GetNewsStatisticByPeriodAsync(
            DateTime startDate,
            DateTime endDate);
    }
}
