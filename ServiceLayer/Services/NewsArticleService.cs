using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface INewsArticleService
    {
        List<NewsArticleModel> GetAllNews();
        List<NewsArticleModel> GetActiveNews();
        NewsArticleModel? GetNewsById(string id);
        List<NewsArticleModel> GetNewsByCreatedBy(short createdById);
        List<NewsArticleModel> GetNewsByDateRange(DateTime startDate, DateTime endDate);
        NewsArticleModel CreateNews(NewsArticleModel newsArticle, List<int>? tagIds);
        void UpdateNews(string id, NewsArticleModel newsArticle, List<int>? tagIds);
        void DeleteNews(string id);
        (List<NewsArticleModel> items, int totalCount) SearchNews(string? title, short? categoryId, bool? status, short? createdById, int page = 1, int pageSize = 10, string? sortBy = null, bool isDescending = false);
    }

    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly ITagRepository _tagRepository;

        public NewsArticleService(INewsArticleRepository newsRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsRepository;
            _tagRepository = tagRepository;
        }

        public List<NewsArticleModel> GetAllNews()
        {
            var entities = _newsRepository.GetAll().ToList();
            return entities.Select(MapToModel).ToList();
        }

        public List<NewsArticleModel> GetActiveNews()
        {
            var entities = _newsRepository.GetActiveNewsArticles().ToList();
            return entities.Select(MapToModel).ToList();
        }

        public NewsArticleModel? GetNewsById(string id)
        {
            var entity = _newsRepository.GetNewsWithDetails().FirstOrDefault(n => n.NewsArticleId == id);
            return entity == null ? null : MapToModelWithDetails(entity);
        }

        public List<NewsArticleModel> GetNewsByCreatedBy(short createdById)
        {
            var entities = _newsRepository.GetByCreatedBy(createdById).ToList();
            return entities.Select(MapToModel).ToList();
        }

        public List<NewsArticleModel> GetNewsByDateRange(DateTime startDate, DateTime endDate)
        {
            var entities = _newsRepository.GetNewsByDateRange(startDate, endDate).ToList();
            return entities.Select(MapToModel).ToList();
        }

        public NewsArticleModel CreateNews(NewsArticleModel newsArticle, List<int>? tagIds)
        {
            var entity = new NewsArticle
            {
                NewsArticleId = newsArticle.NewsArticleId,
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                CategoryId = newsArticle.CategoryId,
                NewsStatus = newsArticle.NewsStatus,
                CreatedById = newsArticle.CreatedById,
                UpdatedById = newsArticle.UpdatedById,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            // Handle tags if provided
            if (tagIds != null && tagIds.Any())
            {
                entity.Tags = _tagRepository.GetAll()
                    .Where(t => tagIds.Contains(t.TagId))
                    .ToList();
            }

            _newsRepository.Add(entity);
            _newsRepository.SaveChanges();

            return MapToModel(entity);
        }

        public void UpdateNews(string id, NewsArticleModel newsArticle, List<int>? tagIds)
        {
            var existingNews = _newsRepository.GetNewsWithDetails()
                .FirstOrDefault(n => n.NewsArticleId == id);

            if (existingNews == null)
            {
                throw new KeyNotFoundException($"News article with ID '{id}' not found");
            }

            existingNews.NewsTitle = newsArticle.NewsTitle;
            existingNews.Headline = newsArticle.Headline;
            existingNews.NewsContent = newsArticle.NewsContent;
            existingNews.NewsSource = newsArticle.NewsSource;
            existingNews.CategoryId = newsArticle.CategoryId;
            existingNews.NewsStatus = newsArticle.NewsStatus;
            existingNews.UpdatedById = newsArticle.UpdatedById;
            existingNews.ModifiedDate = DateTime.Now;

            // Update tags
            if (tagIds != null)
            {
                existingNews.Tags.Clear();
                var tags = _tagRepository.GetAll()
                    .Where(t => tagIds.Contains(t.TagId))
                    .ToList();
                foreach (var tag in tags)
                {
                    existingNews.Tags.Add(tag);
                }
            }

            _newsRepository.SaveChanges();
        }

        public void DeleteNews(string id)
        {
            var news = _newsRepository.GetById(id);
            if (news == null)
            {
                throw new KeyNotFoundException($"News article with ID '{id}' not found");
            }

            _newsRepository.Delete(news);
            _newsRepository.SaveChanges();
        }

        public (List<NewsArticleModel> items, int totalCount) SearchNews(
            string? title, short? categoryId, bool? status, short? createdById,
            int page = 1, int pageSize = 10, string? sortBy = null, bool isDescending = false)
        {
            var query = _newsRepository.GetNewsWithDetails();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(n => n.NewsTitle!.Contains(title) || n.Headline.Contains(title));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryId == categoryId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(n => n.NewsStatus == status.Value);
            }

            if (createdById.HasValue)
            {
                query = query.Where(n => n.CreatedById == createdById.Value);
            }

            // Get total count before pagination
            var totalCount = query.Count();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "title" => isDescending ? query.OrderByDescending(n => n.NewsTitle) : query.OrderBy(n => n.NewsTitle),
                    "createddate" => isDescending ? query.OrderByDescending(n => n.CreatedDate) : query.OrderBy(n => n.CreatedDate),
                    "modifieddate" => isDescending ? query.OrderByDescending(n => n.ModifiedDate) : query.OrderBy(n => n.ModifiedDate),
                    _ => isDescending ? query.OrderByDescending(n => n.CreatedDate) : query.OrderBy(n => n.CreatedDate)
                };
            }
            else
            {
                query = query.OrderByDescending(n => n.CreatedDate);
            }

            // Apply pagination
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(MapToModel)
                .ToList();

            return (items, totalCount);
        }

        private NewsArticleModel MapToModel(NewsArticle entity)
        {
            return new NewsArticleModel
            {
                NewsArticleId = entity.NewsArticleId,
                NewsTitle = entity.NewsTitle,
                Headline = entity.Headline,
                CreatedDate = entity.CreatedDate,
                NewsContent = entity.NewsContent,
                NewsSource = entity.NewsSource,
                CategoryId = entity.CategoryId,
                NewsStatus = entity.NewsStatus,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                ModifiedDate = entity.ModifiedDate,
                TagIds = entity.Tags?.Select(t => t.TagId).ToList() ?? new List<int>()
            };
        }

        private NewsArticleModel MapToModelWithDetails(NewsArticle entity)
        {
            var model = MapToModel(entity);
            // Add additional details if needed
            if (entity.Category != null)
            {
                model.CategoryName = entity.Category.CategoryName;
            }
            if (entity.CreatedBy != null)
            {
                model.CreatedByName = entity.CreatedBy.AccountName;
            }
            return model;
        }
    }
}
