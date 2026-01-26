using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        IQueryable<NewsArticle> GetActiveNewsArticles();
        IQueryable<NewsArticle> GetByCreatedBy(short createdById);
        IQueryable<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate);
        IQueryable<NewsArticle> GetNewsWithDetails();
    }

    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public IQueryable<NewsArticle> GetActiveNewsArticles()
        {
            return _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true && n.Category!.IsActive == true)
                .AsQueryable();
        }

        public IQueryable<NewsArticle> GetByCreatedBy(short createdById)
        {
            return _dbSet
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .Where(n => n.CreatedById == createdById)
                .AsQueryable();
        }

        public IQueryable<NewsArticle> GetNewsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .AsQueryable();
        }

        public IQueryable<NewsArticle> GetNewsWithDetails()
        {
            return _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .AsQueryable();
        }

        public override IQueryable<NewsArticle> GetAll()
        {
            return GetNewsWithDetails();
        }
    }
}
