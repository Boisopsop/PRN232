using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Models.Response.NewsArticle;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Mappers;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NewsArticleDetailResponse?> GetByIdAsync(string id)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            if (newsArticle == null)
            {
                return null;
            }

            return NewsArticleMapper.ToDetailResponse(newsArticle);
        }

        public async Task<PaginatedResponse<NewsArticleResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? status = null,
            short? categoryId = null,
            short? createdById = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var (items, totalCount) = await _unitOfWork.NewsArticles.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                status,
                categoryId,
                createdById,
                fromDate,
                toDate,
                sortBy,
                isDescending);

            var responseItems = items.Select(NewsArticleMapper.ToResponse).ToList();

            return new PaginatedResponse<NewsArticleResponse>(
                responseItems,
                totalCount,
                page,
                pageSize);
        }

        public async Task<List<NewsArticleResponse>> GetActiveNewsAsync()
        {
            var newsArticles = await _unitOfWork.NewsArticles.GetActiveNewsAsync();
            return newsArticles.Select(NewsArticleMapper.ToResponse).ToList();
        }

        public async Task<List<NewsArticleResponse>> GetNewsByStaffAsync(short staffId)
        {
            var newsArticles = await _unitOfWork.NewsArticles.GetNewsByStaffAsync(staffId);
            return newsArticles.Select(NewsArticleMapper.ToResponse).ToList();
        }

        public async Task<NewsArticleDetailResponse> CreateAsync(
            CreateNewsArticleRequest request,
            short createdById)
        {
            // Validate category exists and is active
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryID);
            if (category == null || !category.IsActive)
            {
                throw new InvalidOperationException("Category does not exist or is inactive");
            }

            // Generate new ID
            var newsId = await _unitOfWork.NewsArticles.GenerateNewsIdAsync();

            // Create entity
            var newsArticle = NewsArticleMapper.ToEntity(request, newsId, createdById);

            await _unitOfWork.NewsArticles.AddAsync(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            // Load full details
            var createdNews = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(newsId);
            return NewsArticleMapper.ToDetailResponse(createdNews!);
        }

        public async Task<NewsArticleDetailResponse> UpdateAsync(
            string id,
            UpdateNewsArticleRequest request,
            short updatedById)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            if (newsArticle == null)
            {
                throw new InvalidOperationException("News article not found");
            }

            // Validate category if changed
            if (request.CategoryID != newsArticle.CategoryID)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryID);
                if (category == null || !category.IsActive)
                {
                    throw new InvalidOperationException("Category does not exist or is inactive");
                }
            }

            // Update entity
            NewsArticleMapper.UpdateEntity(newsArticle, request, updatedById);

            _unitOfWork.NewsArticles.Update(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            // Reload with details
            var updatedNews = await _unitOfWork.NewsArticles.GetByIdWithDetailsAsync(id);
            return NewsArticleMapper.ToDetailResponse(updatedNews!);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var newsArticle = await _unitOfWork.NewsArticles.GetByIdAsync(id);
            if (newsArticle == null)
            {
                return false;
            }

            _unitOfWork.NewsArticles.Delete(newsArticle);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
