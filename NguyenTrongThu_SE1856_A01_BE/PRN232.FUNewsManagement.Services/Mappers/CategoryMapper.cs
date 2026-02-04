using PRN232.FUNewsManagement.Models.Entities;
using PRN232.FUNewsManagement.Models.Request.Category;
using PRN232.FUNewsManagement.Models.Response.Category;

namespace PRN232.FUNewsManagement.Services.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToResponse(Category entity)
        {
            return new CategoryResponse
            {
                CategoryID = entity.CategoryID,
                CategoryName = entity.CategoryName,
                CategoryDesciption = entity.CategoryDesciption,
                ParentCategoryID = entity.ParentCategoryID,
                ParentCategoryName = entity.ParentCategory?.CategoryName,
                IsActive = entity.IsActive,
                StatusText = entity.IsActive ? "Active" : "Inactive"
            };
        }

        public static CategoryDetailResponse ToDetailResponse(Category entity)
        {
            return new CategoryDetailResponse
            {
                CategoryID = entity.CategoryID,
                CategoryName = entity.CategoryName,
                CategoryDesciption = entity.CategoryDesciption,
                ParentCategoryID = entity.ParentCategoryID,
                ParentCategoryName = entity.ParentCategory?.CategoryName,
                IsActive = entity.IsActive,
                StatusText = entity.IsActive ? "Active" : "Inactive",
                TotalNewsArticles = entity.NewsArticles?.Count ?? 0,
                ChildCategories = entity.ChildCategories?.Select(ToResponse).ToList() ?? new List<CategoryResponse>()
            };
        }

        public static Category ToEntity(CreateCategoryRequest request)
        {
            return new Category
            {
                CategoryName = request.CategoryName,
                CategoryDesciption = request.CategoryDesciption,
                ParentCategoryID = request.ParentCategoryID,
                IsActive = request.IsActive
            };
        }

        public static void UpdateEntity(Category entity, UpdateCategoryRequest request)
        {
            entity.CategoryName = request.CategoryName;
            entity.CategoryDesciption = request.CategoryDesciption;
            entity.ParentCategoryID = request.ParentCategoryID;
            entity.IsActive = request.IsActive;
        }
    }
}
