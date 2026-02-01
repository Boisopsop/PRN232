using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.Models;

namespace PRN232.FUNewsManagementSystem.Services.Services.IService
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> GetCategoryWithChildrenAsync(int id);
        Task<List<Category>> GetActiveCategoriesAsync();
        Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId);
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
