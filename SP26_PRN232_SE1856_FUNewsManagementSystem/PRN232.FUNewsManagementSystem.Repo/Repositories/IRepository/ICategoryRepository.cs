using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN232.FUNewsManagementSystem.Repo.GenericRepo;
using PRN232.FUNewsManagementSystem.Repo.Models;

namespace PRN232.FUNewsManagementSystem.Repo.Repositories.IRepository
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<List<Category>> GetActiveCategoriesAsync();
        Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId);
        Task<Category> GetCategoryWithChildrenAsync(int id);
    }
}
