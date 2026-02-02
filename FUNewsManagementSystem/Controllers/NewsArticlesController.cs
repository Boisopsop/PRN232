using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RepositoryLayer.Entities;
using ServiceLayer.Services;
using System.Security.Claims;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _newsService;

        public NewsArticlesController(INewsArticleService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] string? title, [FromQuery] short? categoryId, 
            [FromQuery] bool? status, [FromQuery] short? createdById)
        {
            var news = _newsService.SearchNews(title, categoryId, status, createdById);
            return Ok(news);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(string id)
        {
            var news = _newsService.GetNewsById(id);
            if (news == null)
            {
                return NotFound();
            }
            return Ok(news);
        }

        [HttpGet("created-by/me")]
        public IActionResult GetMyNews()
        {
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            var news = _newsService.GetNewsByCreatedBy(accountId);
            return Ok(news);
        }

        [HttpGet("reports")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var news = _newsService.GetNewsByDateRange(startDate, endDate)
                .OrderByDescending(n => n.CreatedDate);
            return Ok(news);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateNewsArticleRequest newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);

            var newsArticle = new NewsArticle
            {
                NewsArticleId = newsDto.NewsArticleId,
                NewsTitle = newsDto.NewsTitle,
                Headline = newsDto.Headline,
                NewsContent = newsDto.NewsContent,
                NewsSource = newsDto.NewsSource,
                CategoryId = newsDto.CategoryId,
                NewsStatus = newsDto.NewsStatus,
                CreatedById = accountId,
                UpdatedById = accountId
            };

            _newsService.CreateNews(newsArticle, newsDto.TagIds);
            return CreatedAtAction(nameof(Get), new { id = newsArticle.NewsArticleId }, newsArticle);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UpdateNewsArticleRequest newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            // Check if user is updating their own news
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            if (existingNews.CreatedById != accountId)
            {
                return Forbid();
            }

            existingNews.NewsTitle = newsDto.NewsTitle;
            existingNews.Headline = newsDto.Headline;
            existingNews.NewsContent = newsDto.NewsContent;
            existingNews.NewsSource = newsDto.NewsSource;
            existingNews.CategoryId = newsDto.CategoryId;
            existingNews.NewsStatus = newsDto.NewsStatus;
            existingNews.UpdatedById = accountId;

            _newsService.UpdateNews(existingNews, newsDto.TagIds);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            // Check if user is deleting their own news
            var accountId = short.Parse(User.FindFirst("AccountId")!.Value);
            if (existingNews.CreatedById != accountId)
            {
                return Forbid();
            }

            _newsService.DeleteNews(id);
            return NoContent();
        }
    }
}
