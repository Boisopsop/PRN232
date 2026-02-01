using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Repo.UnitOfWork;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _unitOfWork.CategoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> GetCategoryWithChildrenAsync(int id)
        {
            return await _unitOfWork.CategoryRepository.GetCategoryWithChildrenAsync(id);
        }

        public async Task<List<Category>> GetActiveCategoriesAsync()
        {
            return await _unitOfWork.CategoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId)
        {
            return await _unitOfWork.CategoryRepository.GetCategoriesByParentIdAsync(parentId);
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            try
            {
                await _unitOfWork.CategoryRepository.CreateAsync(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
                if (category == null)
                    return false;

                await _unitOfWork.CategoryRepository.RemoveAsync(category);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
