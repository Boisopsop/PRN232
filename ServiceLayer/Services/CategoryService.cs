using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        List<Category> GetActiveCategories();
        Category? GetCategoryById(short id);
        void CreateCategory(Category category);
        void UpdateCategory(Category category);
        bool DeleteCategory(short id);
        List<Category> SearchCategories(string? name, bool? isActive);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<Category> GetAllCategories()
        {
            return _repository.GetAll().ToList();
        }

        public List<Category> GetActiveCategories()
        {
            return _repository.GetActiveCategories().ToList();
        }

        public Category? GetCategoryById(short id)
        {
            return _repository.GetById(id);
        }

        public void CreateCategory(Category category)
        {
            _repository.Add(category);
            _repository.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _repository.Update(category);
            _repository.SaveChanges();
        }

        public bool DeleteCategory(short id)
        {
            if (!_repository.CanDeleteCategory(id))
            {
                return false; // Cannot delete category with news articles
            }

            var category = _repository.GetById(id);
            if (category != null)
            {
                _repository.Delete(category);
                _repository.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Category> SearchCategories(string? name, bool? isActive)
        {
            var query = _repository.GetAll();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.CategoryName.Contains(name));
            }

            if (isActive.HasValue)
            {
                query = query.Where(c => c.IsActive == isActive.Value);
            }

            return query.ToList();
        }
    }
}
