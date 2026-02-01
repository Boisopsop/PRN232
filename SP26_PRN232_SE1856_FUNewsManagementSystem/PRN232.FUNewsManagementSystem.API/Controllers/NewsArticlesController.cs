using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetNewsArticles()
        {
            var newsArticles = await _newsArticleService.GetAllNewsArticlesAsync();
            return Ok(newsArticles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsArticle>> GetNewsArticle(int id)
        {
            var newsArticle = await _newsArticleService.GetNewsArticleByIdAsync(id);

            if (newsArticle == null)
            {
                return NotFound(new { message = $"News article with ID {id} not found." });
            }

            return Ok(newsArticle);
        }

        [HttpPost]
        public async Task<ActionResult<NewsArticle>> CreateNewsArticle(NewsArticle newsArticle)
        {
            var result = await _newsArticleService.CreateNewsArticleAsync(newsArticle);

            if (!result)
            {
                return BadRequest(new { message = "Failed to create news article." });
            }

            return CreatedAtAction(nameof(GetNewsArticle), new { id = newsArticle.NewsArticleId }, newsArticle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNewsArticle(int id, NewsArticle newsArticle)
        {
            if (id != newsArticle.NewsArticleId)
            {
                return BadRequest(new { message = "News article ID mismatch." });
            }

            var result = await _newsArticleService.UpdateNewsArticleAsync(newsArticle);

            if (!result)
            {
                return NotFound(new { message = $"News article with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewsArticle(int id)
        {
            var result = await _newsArticleService.DeleteNewsArticleAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"News article with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> SearchNewsArticles(
            [FromQuery] string? title,
            [FromQuery] int? categoryId,
            [FromQuery] int? createdById)
        {
            var newsArticles = await _newsArticleService.GetAllNewsArticlesAsync();

            if (!string.IsNullOrEmpty(title))
            {
                newsArticles = newsArticles.Where(n => n.NewsTitle != null &&
                    n.NewsTitle.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categoryId.HasValue)
            {
                newsArticles = newsArticles.Where(n => n.CategoryId == categoryId.Value).ToList();
            }

            if (createdById.HasValue)
            {
                newsArticles = newsArticles.Where(n => n.CreatedById == createdById.Value).ToList();
            }

            return Ok(newsArticles);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetNewsArticlesByCategory(int categoryId)
        {
            var newsArticles = await _newsArticleService.GetNewsArticlesByCategoryAsync(categoryId);
            return Ok(newsArticles);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetNewsArticlesByAuthor(int authorId)
        {
            var newsArticles = await _newsArticleService.GetNewsArticlesByAuthorAsync(authorId);
            return Ok(newsArticles);
        }

    }
}
