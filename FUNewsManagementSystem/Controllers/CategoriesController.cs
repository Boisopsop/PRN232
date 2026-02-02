using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RepositoryLayer.Entities;
using ServiceLayer.Services;

namespace FUNewsManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] bool? isActive, [FromQuery] string? name)
        {
            // If both filters are provided
            if (isActive.HasValue || !string.IsNullOrEmpty(name))
            {
                var categories = _categoryService.SearchCategories(name, isActive);
                return Ok(categories);
            }
            
            // Default: return all categories
            var allCategories = _categoryService.GetAllCategories();
            return Ok(allCategories);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult Get(short id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateCategoryRequest categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate ParentCategoryId if provided
            if (categoryDto.ParentCategoryId.HasValue)
            {
                var parentCategory = _categoryService.GetCategoryById(categoryDto.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    return BadRequest(new { message = "Parent category does not exist" });
                }
            }

            var category = new Category
            {
                CategoryName = categoryDto.CategoryName,
                CategoryDesciption = categoryDto.CategoryDesciption,
                ParentCategoryId = categoryDto.ParentCategoryId,
                IsActive = categoryDto.IsActive
            };

            _categoryService.CreateCategory(category);
            return CreatedAtAction(nameof(Get), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Put(short id, [FromBody] UpdateCategoryRequest categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Validate ParentCategoryId if provided
            if (categoryDto.ParentCategoryId.HasValue && categoryDto.ParentCategoryId.Value != id)
            {
                var parentCategory = _categoryService.GetCategoryById(categoryDto.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    return BadRequest(new { message = "Parent category does not exist" });
                }
            }
            else if (categoryDto.ParentCategoryId.HasValue && categoryDto.ParentCategoryId.Value == id)
            {
                return BadRequest(new { message = "A category cannot be its own parent" });
            }

            existingCategory.CategoryName = categoryDto.CategoryName;
            existingCategory.CategoryDesciption = categoryDto.CategoryDesciption;
            existingCategory.ParentCategoryId = categoryDto.ParentCategoryId;
            existingCategory.IsActive = categoryDto.IsActive;

            _categoryService.UpdateCategory(existingCategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(short id)
        {
            var result = _categoryService.DeleteCategory(id);
            if (!result)
            {
                return BadRequest(new { message = "Cannot delete category with existing news articles" });
            }
            return NoContent();
        }
    }
}
