using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            var result = categories.Select(c => new
            {
                c.CategoryId,
                c.CategoryName,
                c.CategoryDescription,
                c.ParentCategoryId,
                c.IsActive
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            var result = new
            {
                category.CategoryId,
                category.CategoryName,
                category.CategoryDescription,
                category.ParentCategoryId,
                category.IsActive
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            var result = await _categoryService.CreateCategoryAsync(category);

            if (!result)
            {
                return BadRequest(new { message = "Failed to create category." });
            }

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest(new { message = "Category ID mismatch." });
            }

            var result = await _categoryService.UpdateCategoryAsync(category);

            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Category with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<object>>> SearchCategories([FromQuery] string? name)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (!string.IsNullOrEmpty(name))
            {
                categories = categories.Where(c => c.CategoryName != null &&
                    c.CategoryName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var result = categories.Select(c => new
            {
                c.CategoryId,
                c.CategoryName,
                c.CategoryDescription,
                c.ParentCategoryId,
                c.IsActive
            });
            return Ok(result);
        }
    }
}
