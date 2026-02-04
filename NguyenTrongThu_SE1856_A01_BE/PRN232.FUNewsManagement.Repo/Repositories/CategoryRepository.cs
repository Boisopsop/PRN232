using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Repo.Data;
using PRN232.FUNewsManagement.Repo.Interfaces;

namespace PRN232.FUNewsManagement.Repo.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public async Task<bool> HasNewsArticlesAsync(short categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryID == categoryId);
        }

        public async Task<bool> NameExistsAsync(string name, short? excludeCategoryId = null)
        {
            return await _dbSet.AnyAsync(c => 
                c.CategoryName == name && 
                (!excludeCategoryId.HasValue || c.CategoryID != excludeCategoryId.Value));
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .Include(c => c.ParentCategory)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(short parentId)
        {
            return await _dbSet
                .Where(c => c.ParentCategoryID == parentId)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdWithDetailsAsync(short id)
        {
            return await _dbSet
                .Include(c => c.ParentCategory)
                .Include(c => c.ChildCategories)
                .Include(c => c.NewsArticles)
                .FirstOrDefaultAsync(c => c.CategoryID == id);
        }

        public async Task<(IEnumerable<Category> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? isActive = null,
            short? parentCategoryId = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var query = _dbSet
                .Include(c => c.ParentCategory)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.CategoryName.ToLower().Contains(searchTerm) ||
                    c.CategoryDesciption.ToLower().Contains(searchTerm));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            if (parentCategoryId.HasValue)
            {
                query = query.Where(c => c.ParentCategoryID == parentCategoryId.Value);
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

        private IQueryable<Category> ApplySorting(
            IQueryable<Category> query,
            string? sortBy,
            bool isDescending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return query.OrderBy(c => c.CategoryName);
            }

            query = sortBy.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(c => c.CategoryName)
                    : query.OrderBy(c => c.CategoryName),
                "status" => isDescending
                    ? query.OrderByDescending(c => c.IsActive)
                    : query.OrderBy(c => c.IsActive),
                _ => query.OrderBy(c => c.CategoryName)
            };

            return query;
        }
    }
}
