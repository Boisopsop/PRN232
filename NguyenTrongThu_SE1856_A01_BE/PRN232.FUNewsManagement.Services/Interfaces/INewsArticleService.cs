using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<NewsArticleDetailResponse?> GetByIdAsync(string id);
        Task<PaginatedResponse<NewsArticleResponse>> GetPagedAsync(
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
        Task<List<NewsArticleResponse>> GetActiveNewsAsync();
        Task<List<NewsArticleResponse>> GetNewsByStaffAsync(short staffId);
        Task<NewsArticleDetailResponse> CreateAsync(CreateNewsArticleRequest request, short createdById);
        Task<NewsArticleDetailResponse> UpdateAsync(string id, UpdateNewsArticleRequest request, short updatedById);
        Task<bool> DeleteAsync(string id);
    }
}
