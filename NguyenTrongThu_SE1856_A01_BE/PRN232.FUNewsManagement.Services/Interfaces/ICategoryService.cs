using PRN232.FUNewsManagement.Models.Request.Category;
using PRN232.FUNewsManagement.Models.Response.Category;
using PRN232.FUNewsManagement.Models.Response.Common;

namespace PRN232.FUNewsManagement.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDetailResponse?> GetByIdAsync(short id);
        Task<PaginatedResponse<CategoryResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? isActive = null,
            short? parentCategoryId = null,
            string? sortBy = null,
            bool isDescending = false);
        Task<List<CategoryResponse>> GetActiveCategoriesAsync();
        Task<CategoryDetailResponse> CreateAsync(CreateCategoryRequest request);
        Task<CategoryDetailResponse> UpdateAsync(short id, UpdateCategoryRequest request);
        Task<bool> DeleteAsync(short id);
    }
}
