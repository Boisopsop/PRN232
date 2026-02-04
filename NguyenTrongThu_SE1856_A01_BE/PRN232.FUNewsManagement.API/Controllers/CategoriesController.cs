using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Category;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get paginated list of categories with filtering, sorting, and searching
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] bool? isActive = null,
            [FromQuery] short? parentCategoryId = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            var result = await _categoryService.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                isActive,
                parentCategoryId,
                sortBy,
                isDescending);

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get category by ID with full details
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(short id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.FailureResult("Category not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get active categories
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveCategories()
        {
            var result = await _categoryService.GetActiveCategoriesAsync();
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Create new category
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            try
            {
                var result = await _categoryService.CreateAsync(request);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.CategoryID },
                    ApiResponse<object>.SuccessResult(result, "Category created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Update category
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(short id, [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<object>.FailureResult("Validation failed", errors));
            }

            try
            {
                var result = await _categoryService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.SuccessResult(result, "Category updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Delete category
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var result = await _categoryService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<object>.FailureResult("Category not found"));
                }

                return Ok(new ApiResponse<object> { Success = true, Message = "Category deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
