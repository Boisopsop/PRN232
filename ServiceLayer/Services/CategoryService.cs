using RepositoryLayer.Entities;
using RepositoryLayer.Repositories;
using ServiceLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLayer.Services
{
    public interface ICategoryService
    {
        List<CategoryModel> GetAllCategories();
        List<CategoryModel> GetActiveCategories();
        CategoryModel? GetCategoryById(short id);
        CategoryModel CreateCategory(CategoryModel category);
        void UpdateCategory(short id, CategoryModel category);
        bool DeleteCategory(short id);
        (List<CategoryModel> items, int totalCount) SearchCategories(string? name, bool? isActive, int page = 1, int pageSize = 10);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public List<CategoryModel> GetAllCategories()
        {
            var entities = _repository.GetAll().ToList();
            return entities.Select(MapToModel).ToList();
        }

        public List<CategoryModel> GetActiveCategories()
        {
            var entities = _repository.GetActiveCategories().ToList();
            return entities.Select(MapToModel).ToList();
        }

        public CategoryModel? GetCategoryById(short id)
        {
            var entity = _repository.GetById(id);
            return entity == null ? null : MapToModel(entity);
        }

        public CategoryModel CreateCategory(CategoryModel category)
        {
            var entity = new Category
            {
                CategoryName = category.CategoryName,
                CategoryDesciption = category.CategoryDesciption,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive ?? true
            };

            _repository.Add(entity);
            _repository.SaveChanges();

            return MapToModel(entity);
        }

        public void UpdateCategory(short id, CategoryModel category)
        {
            var entity = _repository.GetById(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found");
            }

            entity.CategoryName = category.CategoryName;
            entity.CategoryDesciption = category.CategoryDesciption;
            entity.ParentCategoryId = category.ParentCategoryId;
            entity.IsActive = category.IsActive ?? true;

            _repository.Update(entity);
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

        public (List<CategoryModel> items, int totalCount) SearchCategories(string? name, bool? isActive, int page = 1, int pageSize = 10)
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

            var totalCount = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList()
                .Select(MapToModel)
                .ToList();

            return (items, totalCount);
        }

        private CategoryModel MapToModel(Category entity)
        {
            return new CategoryModel
            {
                CategoryId = entity.CategoryId,
                CategoryName = entity.CategoryName,
                CategoryDesciption = entity.CategoryDesciption,
                ParentCategoryId = entity.ParentCategoryId,
                IsActive = entity.IsActive
            };
        }
    }
}
