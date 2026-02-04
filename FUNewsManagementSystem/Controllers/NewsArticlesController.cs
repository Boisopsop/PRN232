using FUNewsManagementSystem.Models.Common;
using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Services;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    /// <summary>
    /// RESTful API Controller for News Articles
    /// Follows 3-layer architecture: Controller -> Service -> Repository
    /// </summary>
    [Route("api/v1/news-articles")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsService;

        public NewsArticlesController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// GET /api/news-articles - Get paginated list of news articles with search, filter, sort support
        /// </summary>
        /// <param name="title">Search by title or headline</param>
        /// <param name="categoryId">Filter by category</param>
        /// <param name="status">Filter by status (active/inactive)</param>
        /// <param name="createdById">Filter by creator</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <param name="sortBy">Sort field: title, createdDate, modifiedDate</param>
        /// <param name="isDescending">Sort direction (default: false)</param>
        /// <returns>Paginated list of news articles</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<NewsArticleResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetNewsArticles(
            [FromQuery] string? title,
            [FromQuery] short? categoryId,
            [FromQuery] bool? status,
            [FromQuery] short? createdById,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var (items, totalCount) = _newsService.SearchNews(
                title, categoryId, status, createdById,
                page, pageSize, sortBy, isDescending);

            var responseItems = items.Select(MapToResponse).ToList();
            var paginatedData = new PaginatedResponse<NewsArticleResponse>(
                responseItems, totalCount, page, pageSize);

            return Ok(ApiResponse<PaginatedResponse<NewsArticleResponse>>.SuccessResponse(
                paginatedData,
                "News articles retrieved successfully"));
        }

        /// <summary>
        /// GET /api/news-articles/{id} - Get single news article by ID with full details
        /// </summary>
        /// <param name="id">News article ID</param>
        /// <returns>News article details</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public IActionResult GetNewsArticleById(string id)
        {
            var news = _newsService.GetNewsById(id);
            if (news == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"News article with ID '{id}' not found"));
            }

            var response = MapToResponse(news);
            return Ok(ApiResponse<NewsArticleResponse>.SuccessResponse(
                response,
                "News article retrieved successfully"));
        }

        /// <summary>
        /// GET /api/news-articles/me - Get news articles created by current user
        /// </summary>
        /// <returns>List of user's news articles</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<NewsArticleResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetMyNewsArticles()
        {
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            var news = _newsService.GetNewsByCreatedBy(accountId);
            var response = news.Select(MapToResponse).ToList();

            return Ok(ApiResponse<List<NewsArticleResponse>>.SuccessResponse(
                response,
                "Your news articles retrieved successfully"));
        }

        /// <summary>
        /// GET /api/news-articles/reports - Get news articles within date range (Admin only)
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>News articles in date range</returns>
        [HttpGet("reports")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<NewsArticleResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var news = _newsService.GetNewsByDateRange(startDate, endDate);
            var response = news.Select(MapToResponse).ToList();

            return Ok(ApiResponse<List<NewsArticleResponse>>.SuccessResponse(
                response,
                $"Report retrieved for period {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}"));
        }

        /// <summary>
        /// POST /api/news-articles - Create new news article
        /// </summary>
        /// <param name="request">News article creation data</param>
        /// <returns>Created news article</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<NewsArticleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewsArticle([FromBody] CreateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(new { errors });
            }

            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);

            var newsModel = new NewsArticleModel
            {
                NewsArticleId = request.NewsArticleId,
                NewsTitle = request.NewsTitle,
                Headline = request.Headline,
                NewsContent = request.NewsContent,
                NewsSource = request.NewsSource,
                CategoryId = request.CategoryId,
                NewsStatus = request.NewsStatus ?? true,
                CreatedById = accountId,
                UpdatedById = accountId
            };

            var createdNews = _newsService.CreateNews(newsModel, request.TagIds);
            var response = MapToResponse(createdNews);

            return CreatedAtAction(
                nameof(GetNewsArticleById),
                new { id = response.NewsArticleId },
                ApiResponse<NewsArticleResponse>.SuccessResponse(
                    response,
                    "News article created successfully"));
        }

        /// <summary>
        /// PUT /api/news-articles/{id} - Update existing news article
        /// </summary>
        /// <param name="id">News article ID</param>
        /// <param name="request">Updated news article data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult UpdateNewsArticle(string id, [FromBody] UpdateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    errors));
            }

            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"News article with ID '{id}' not found"));
            }

            // Check authorization
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            if (existingNews.CreatedById != accountId)
            {
                return Forbid();
            }

            var updateModel = new NewsArticleModel
            {
                NewsTitle = request.NewsTitle,
                Headline = request.Headline,
                NewsContent = request.NewsContent,
                NewsSource = request.NewsSource,
                CategoryId = request.CategoryId,
                NewsStatus = request.NewsStatus,
                UpdatedById = accountId
            };

            _newsService.UpdateNews(id, updateModel, request.TagIds);

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "News article updated successfully"));
        }

        /// <summary>
        /// DELETE /api/news-articles/{id} - Delete news article
        /// </summary>
        /// <param name="id">News article ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DeleteNewsArticle(string id)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"News article with ID '{id}' not found"));
            }

            // Check authorization
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            if (existingNews.CreatedById != accountId)
            {
                return Forbid();
            }

            _newsService.DeleteNews(id);

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "News article deleted successfully"));
        }

        /// <summary>
        /// Maps Business Model to Response Model
        /// </summary>
        private NewsArticleResponse MapToResponse(NewsArticleModel model)
        {
            return new NewsArticleResponse
            {
                NewsArticleId = model.NewsArticleId,
                NewsTitle = model.NewsTitle,
                Headline = model.Headline,
                CreatedDate = model.CreatedDate,
                NewsContent = model.NewsContent,
                NewsSource = model.NewsSource,
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                NewsStatus = model.NewsStatus,
                CreatedById = model.CreatedById,
                CreatedByName = model.CreatedByName,
                UpdatedById = model.UpdatedById,
                ModifiedDate = model.ModifiedDate,
                TagIds = model.TagIds
            };
        }
    }
}
