using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetAllNews();
        List<NewsArticle> GetActiveNews();
        NewsArticle? GetNewsById(string id);
        List<NewsArticle> GetNewsByCreatedBy(short createdById);
        List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate);
        void CreateNews(NewsArticle newsArticle, List<int>? tagIds);
        void UpdateNews(NewsArticle newsArticle, List<int>? tagIds);
        void DeleteNews(string id);
        List<NewsArticle> SearchNews(string? title, short? categoryId, bool? status, short? createdById);
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

        public List<NewsArticle> GetAllNews()
        {
            return _newsRepository.GetAll().ToList();
        }

        public List<NewsArticle> GetActiveNews()
        {
            return _newsRepository.GetActiveNewsArticles().ToList();
        }

        public NewsArticle? GetNewsById(string id)
        {
            return _newsRepository.GetNewsWithDetails().FirstOrDefault(n => n.NewsArticleId == id);
        }

        public List<NewsArticle> GetNewsByCreatedBy(short createdById)
        {
            return _newsRepository.GetByCreatedBy(createdById).ToList();
        }

        public List<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _newsRepository.GetNewsByDateRange(startDate, endDate).ToList();
        }

        public void CreateNews(NewsArticle newsArticle, List<int>? tagIds)
        {
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.ModifiedDate = DateTime.Now;

            // Handle tags if provided
            if (tagIds != null && tagIds.Any())
            {
                newsArticle.Tags = _tagRepository.GetAll()
                    .Where(t => tagIds.Contains(t.TagId))
                    .ToList();
            }

            _newsRepository.Add(newsArticle);
            _newsRepository.SaveChanges();
        }

        public void UpdateNews(NewsArticle newsArticle, List<int>? tagIds)
        {
            var existingNews = _newsRepository.GetNewsWithDetails()
                .FirstOrDefault(n => n.NewsArticleId == newsArticle.NewsArticleId);

            if (existingNews != null)
            {
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
        }

        public void DeleteNews(string id)
        {
            var news = _newsRepository.GetById(id);
            if (news != null)
            {
                _newsRepository.Delete(news);
                _newsRepository.SaveChanges();
            }
        }

        public List<NewsArticle> SearchNews(string? title, short? categoryId, bool? status, short? createdById)
        {
            var query = _newsRepository.GetNewsWithDetails();

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

            return query.ToList();
        }
    }
}
