using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.API.Models.Common;
using PRN232.FUNewsManagementSystem.API.Models.Request;
using PRN232.FUNewsManagementSystem.API.Models.Response;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [Route("api/news-articles")]
    [ApiController]
    [Authorize]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PagedResponse<NewsArticleListResponse>>>> GetNewsArticles(
            [FromQuery] NewsArticleQueryRequest query)
        {
            var allArticles = await _newsArticleService.GetAllNewsArticlesAsync();

            // Search
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                allArticles = allArticles.Where(n =>
                    (n.NewsTitle != null && n.NewsTitle.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (n.NewsContent != null && n.NewsContent.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Filter by Category
            if (query.CategoryId.HasValue)
            {
                allArticles = allArticles.Where(n => n.CategoryId == query.CategoryId.Value).ToList();
            }

            // Filter by Status
            if (query.Status.HasValue)
            {
                allArticles = allArticles.Where(n => n.NewsStatus == query.Status.Value).ToList();
            }

            // Filter by Author
            if (query.CreatedById.HasValue)
            {
                allArticles = allArticles.Where(n => n.CreatedById == query.CreatedById.Value).ToList();
            }

            // Sorting
            allArticles = query.SortBy?.ToLower() switch
            {
                "title" => query.SortOrder?.ToLower() == "asc"
                    ? allArticles.OrderBy(n => n.NewsTitle).ToList()
                    : allArticles.OrderByDescending(n => n.NewsTitle).ToList(),
                "createddate" => query.SortOrder?.ToLower() == "asc"
                    ? allArticles.OrderBy(n => n.CreatedDate).ToList()
                    : allArticles.OrderByDescending(n => n.CreatedDate).ToList(),
                _ => allArticles.OrderByDescending(n => n.CreatedDate).ToList()
            };

            // Total items before pagination
            var totalItems = allArticles.Count;

            // Pagination
            var paginatedArticles = allArticles
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            // Map to Response
            var responseItems = paginatedArticles.Select(n => new NewsArticleListResponse
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                CreatedDate = n.CreatedDate,
                CategoryName = n.Category?.CategoryName,
                CreatedByName = n.CreatedBy?.AccountName,
                NewsStatus = n.NewsStatus
            }).ToList();

            var pagedResponse = new PagedResponse<NewsArticleListResponse>(
                responseItems,
                query.Page,
                query.PageSize,
                totalItems
            );

            return Ok(ApiResponse<PagedResponse<NewsArticleListResponse>>.SuccessResponse(pagedResponse));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<NewsArticleResponse>>> GetNewsArticle(int id)
        {
            var newsArticle = await _newsArticleService.GetNewsArticleWithDetailsAsync(id);

            if (newsArticle == null)
            {
                return NotFound(ApiResponse<NewsArticleResponse>.ErrorResponse($"News article with ID {id} not found"));
            }

            var response = new NewsArticleResponse
            {
                NewsArticleId = newsArticle.NewsArticleId,
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                CreatedDate = newsArticle.CreatedDate,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                CategoryId = newsArticle.CategoryId,
                CategoryName = newsArticle.Category?.CategoryName,
                NewsStatus = newsArticle.NewsStatus,
                CreatedById = newsArticle.CreatedById,
                CreatedByName = newsArticle.CreatedBy?.AccountName,
                UpdatedById = newsArticle.UpdatedById,
                UpdatedByName = newsArticle.UpdatedBy?.AccountName,
                ModifiedDate = newsArticle.ModifiedDate,
                Tags = newsArticle.Tags?.Select(t => new TagResponse
                {
                    TagId = t.TagId,
                    TagName = t.TagName,
                    Note = t.Note
                }).ToList() ?? new List<TagResponse>()
            };

            return Ok(ApiResponse<NewsArticleResponse>.SuccessResponse(response));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<NewsArticleResponse>>> CreateNewsArticle(
            [FromBody] CreateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<NewsArticleResponse>.ErrorResponse("Validation failed", errors));
            }

            var newsArticle = new Repo.Models.NewsArticle
            {
                NewsTitle = request.NewsTitle,
                Headline = request.Headline,
                NewsContent = request.NewsContent,
                NewsSource = request.NewsSource,
                CategoryId = request.CategoryId,
                NewsStatus = request.NewsStatus,
                CreatedById = request.CreatedById
            };

            var result = await _newsArticleService.CreateNewsArticleAsync(newsArticle);

            if (!result)
            {
                return BadRequest(ApiResponse<NewsArticleResponse>.ErrorResponse("Failed to create news article"));
            }

            var createdArticle = await _newsArticleService.GetNewsArticleWithDetailsAsync(newsArticle.NewsArticleId);

            var response = new NewsArticleResponse
            {
                NewsArticleId = createdArticle.NewsArticleId,
                NewsTitle = createdArticle.NewsTitle,
                Headline = createdArticle.Headline,
                CreatedDate = createdArticle.CreatedDate,
                NewsContent = createdArticle.NewsContent,
                CategoryName = createdArticle.Category?.CategoryName
            };

            return CreatedAtAction(nameof(GetNewsArticle),
                new { id = newsArticle.NewsArticleId },
                ApiResponse<NewsArticleResponse>.SuccessResponse(response, "News article created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> UpdateNewsArticle(int id,
            [FromBody] UpdateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            var existingArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"News article with ID {id} not found"));
            }

            existingArticle.NewsTitle = request.NewsTitle;
            existingArticle.Headline = request.Headline;
            existingArticle.NewsContent = request.NewsContent;
            existingArticle.NewsSource = request.NewsSource;
            existingArticle.CategoryId = request.CategoryId;
            existingArticle.NewsStatus = request.NewsStatus;
            existingArticle.UpdatedById = request.UpdatedById;

            var result = await _newsArticleService.UpdateNewsArticleAsync(existingArticle);

            if (!result)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Failed to update news article"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "News article updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteNewsArticle(int id)
        {
            var result = await _newsArticleService.DeleteNewsArticleAsync(id);

            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse($"News article with ID {id} not found"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "News article deleted successfully"));
        }
    }
}