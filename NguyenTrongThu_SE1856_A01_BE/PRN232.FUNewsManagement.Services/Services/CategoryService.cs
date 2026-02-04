using PRN232.FUNewsManagement.Models.Request.Category;
using PRN232.FUNewsManagement.Models.Response.Category;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Repo.Interfaces;
using PRN232.FUNewsManagement.Services.Interfaces;
using PRN232.FUNewsManagement.Services.Mappers;

namespace PRN232.FUNewsManagement.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDetailResponse?> GetByIdAsync(short id)
        {
            var category = await _unitOfWork.Categories.GetByIdWithDetailsAsync(id);
            if (category == null)
            {
                return null;
            }

            return CategoryMapper.ToDetailResponse(category);
        }

        public async Task<PaginatedResponse<CategoryResponse>> GetPagedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            bool? isActive = null,
            short? parentCategoryId = null,
            string? sortBy = null,
            bool isDescending = false)
        {
            var (items, totalCount) = await _unitOfWork.Categories.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                isActive,
                parentCategoryId,
                sortBy,
                isDescending);

            var responseItems = items.Select(CategoryMapper.ToResponse).ToList();

            return new PaginatedResponse<CategoryResponse>(
                responseItems,
                totalCount,
                page,
                pageSize);
        }

        public async Task<List<CategoryResponse>> GetActiveCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
            return categories.Select(CategoryMapper.ToResponse).ToList();
        }

        public async Task<CategoryDetailResponse> CreateAsync(CreateCategoryRequest request)
        {
            // Validate parent category if specified
            if (request.ParentCategoryID.HasValue)
            {
                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentCategoryID.Value);
                if (parentCategory == null)
                {
                    throw new InvalidOperationException("Parent category does not exist");
                }
            }

            var category = CategoryMapper.ToEntity(request);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            // Reload with details
            var createdCategory = await _unitOfWork.Categories.GetByIdWithDetailsAsync(category.CategoryID);
            return CategoryMapper.ToDetailResponse(createdCategory!);
        }

        public async Task<CategoryDetailResponse> UpdateAsync(short id, UpdateCategoryRequest request)
        {
            var category = await _unitOfWork.Categories.GetByIdWithDetailsAsync(id);
            if (category == null)
            {
                throw new InvalidOperationException("Category not found");
            }

            // Validate parent category if specified
            if (request.ParentCategoryID.HasValue)
            {
                if (request.ParentCategoryID.Value == id)
                {
                    throw new InvalidOperationException("Category cannot be its own parent");
                }

                var parentCategory = await _unitOfWork.Categories.GetByIdAsync(request.ParentCategoryID.Value);
                if (parentCategory == null)
                {
                    throw new InvalidOperationException("Parent category does not exist");
                }
            }

            CategoryMapper.UpdateEntity(category, request);

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            // Reload with details
            var updatedCategory = await _unitOfWork.Categories.GetByIdWithDetailsAsync(id);
            return CategoryMapper.ToDetailResponse(updatedCategory!);
        }

        public async Task<bool> DeleteAsync(short id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            // Check if category has news articles
            if (await _unitOfWork.Categories.HasNewsArticlesAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category with linked news articles");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
