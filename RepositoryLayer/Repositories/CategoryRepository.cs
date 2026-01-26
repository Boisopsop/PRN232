using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System.Linq;

namespace RepositoryLayer.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        bool CanDeleteCategory(short categoryId);
        IQueryable<Category> GetActiveCategories();
    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementContext context) : base(context)
        {
        }

        public bool CanDeleteCategory(short categoryId)
        {
            // Check if category is used in any news articles
            return !_context.NewsArticles.Any(n => n.CategoryId == categoryId);
        }

        public IQueryable<Category> GetActiveCategories()
        {
            return _dbSet.Where(c => c.IsActive == true);
        }

        public override IQueryable<Category> GetAll()
        {
            return _dbSet.Include(c => c.ParentCategory).AsQueryable();
        }
    }
}
