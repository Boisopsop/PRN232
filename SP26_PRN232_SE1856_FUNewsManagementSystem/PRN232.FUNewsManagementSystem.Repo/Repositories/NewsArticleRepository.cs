using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsDBContext context) : base(context)
        {
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByCategoryAsync(int categoryId)
        {
            return await _context.NewsArticles
                .Where(n => n.CategoryId == categoryId)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByStatusAsync(int status)
        {
            return await _context.NewsArticles
                .Where(n => n.NewsStatus == status)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle> GetNewsArticleWithDetailsAsync(int id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == id);
        }

        public async Task<List<NewsArticle>> GetNewsArticlesByAuthorAsync(int authorId)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedById == authorId)
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetRecentNewsArticlesAsync(int count)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .OrderByDescending(n => n.CreatedDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
