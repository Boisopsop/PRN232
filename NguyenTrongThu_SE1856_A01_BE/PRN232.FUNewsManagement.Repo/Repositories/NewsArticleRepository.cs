using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class NewsArticleRepository : BaseRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id);
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.NewsStatus == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByStaffAsync(short staffId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Where(n => n.CreatedByID == staffId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<string> GenerateNewsIdAsync()
        {
            var lastNews = await _dbSet
                .OrderByDescending(n => n.NewsArticleID)
                .FirstOrDefaultAsync();

            if (lastNews == null)
            {
                return "NEWS0001";
            }

            var lastIdNumber = lastNews.NewsArticleID.Replace("NEWS", "");
            if (int.TryParse(lastIdNumber, out int lastNumber))
            {
                var newNumber = lastNumber + 1;
                return $"NEWS{newNumber:D4}";
            }

            return "NEWS0001";
        }

        public async Task<(IEnumerable<NewsArticle> items, int totalCount)> GetPagedAsync(
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
            var query = _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(n =>
                    n.NewsTitle.ToLower().Contains(searchTerm) ||
                    n.Headline.ToLower().Contains(searchTerm) ||
                    n.NewsContent.ToLower().Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(n => n.NewsStatus == status.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(n => n.CategoryID == categoryId.Value);
            }

            if (createdById.HasValue)
            {
                query = query.Where(n => n.CreatedByID == createdById.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(n => n.CreatedDate <= toDate.Value);
            }

            // Sorting
            query = ApplySorting(query, sortBy, isDescending);

            var totalCount = await query.CountAsync();

            // Pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsStatisticByPeriodAsync(
            DateTime startDate,
            DateTime endDate)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        private IQueryable<NewsArticle> ApplySorting(
            IQueryable<NewsArticle> query,
            string? sortBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query.OrderByDescending(n => n.CreatedDate);
            }

            query = sortBy.ToLower() switch
            {
                "title" => isDescending
                    ? query.OrderByDescending(n => n.NewsTitle)
                    : query.OrderBy(n => n.NewsTitle),
                "createddate" => isDescending
                    ? query.OrderByDescending(n => n.CreatedDate)
                    : query.OrderBy(n => n.CreatedDate),
                "category" => isDescending
                    ? query.OrderByDescending(n => n.Category.CategoryName)
                    : query.OrderBy(n => n.Category.CategoryName),
                _ => query.OrderByDescending(n => n.CreatedDate)
            };

            return query;
        }
    }
}
