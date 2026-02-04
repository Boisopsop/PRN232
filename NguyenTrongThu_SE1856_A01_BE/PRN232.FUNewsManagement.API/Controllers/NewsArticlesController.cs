using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.NewsArticle;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;
using System.Security.Claims;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        /// <summary>
        /// Get paginated list of news articles with filtering, sorting, and searching
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool? status = null,
            [FromQuery] short? categoryId = null,
            [FromQuery] short? createdById = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            var result = await _newsArticleService.GetPagedAsync(
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

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get news article by ID with full details
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _newsArticleService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.FailureResult("News article not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get active news articles (public access)
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveNews()
        {
            var result = await _newsArticleService.GetActiveNewsAsync();
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get news articles created by current staff
        /// </summary>
        [HttpGet("my-news")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyNews()
        {
            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _newsArticleService.GetNewsByStaffAsync(accountId);
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Create new news article
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            try
            {
                var result = await _newsArticleService.CreateAsync(request, accountId);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.NewsArticleID },
                    ApiResponse<object>.SuccessResult(result, "News article created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Update news article
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateNewsArticleRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            var accountId = short.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            
            try
            {
                var result = await _newsArticleService.UpdateAsync(id, request, accountId);
                return Ok(ApiResponse<object>.SuccessResult(result, "News article updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Delete news article
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "1")] // Staff only
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _newsArticleService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<object>.FailureResult("News article not found"));
            }

            return Ok(new ApiResponse<object> { Success = true, Message = "News article deleted successfully" });
        }
    }
}
