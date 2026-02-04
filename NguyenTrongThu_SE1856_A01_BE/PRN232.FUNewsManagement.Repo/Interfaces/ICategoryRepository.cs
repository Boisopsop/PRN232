using PRN232.FUNewsManagement.Models.Entities;

namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<bool> HasNewsArticlesAsync(short categoryId);
        Task<bool> NameExistsAsync(string name, short? excludeCategoryId = null);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetChildCategoriesAsync(short parentId);
        Task<Category?> GetByIdWithDetailsAsync(short id);
        Task<(IEnumerable<Category> items, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? isActive = null,
            short? parentCategoryId = null,
            string? sortBy = null,
            bool isDescending = false);
    }
}
