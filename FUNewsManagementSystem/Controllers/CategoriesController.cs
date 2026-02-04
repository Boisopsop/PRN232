using FUNewsManagementSystem.Models.Common;
using FUNewsManagementSystem.Models.Requests;
using FUNewsManagementSystem.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Models;
using ServiceLayer.Services;

namespace FUNewsManagementSystem.Controllers
{
    /// <summary>
    /// RESTful API Controller for Categories
    /// Follows 3-layer architecture: Controller -> Service -> Repository
    /// </summary>
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// GET /api/categories - Get paginated list of categories with search and filter support
        /// </summary>
        /// <param name="name">Search by category name</param>
        /// <param name="isActive">Filter by active status</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <returns>Paginated list of categories</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CategoryResponse>>), StatusCodes.Status200OK)]
        public IActionResult GetCategories(
            [FromQuery] string? name,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var (items, totalCount) = _categoryService.SearchCategories(name, isActive, page, pageSize);

            var responseItems = items.Select(MapToResponse).ToList();
            var paginatedData = new PaginatedResponse<CategoryResponse>(
                responseItems, totalCount, page, pageSize);

            return Ok(ApiResponse<PaginatedResponse<CategoryResponse>>.SuccessResponse(
                paginatedData,
                "Categories retrieved successfully"));
        }

        /// <summary>
        /// GET /api/categories/{id} - Get single category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category details</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById(short id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Category with ID {id} not found"));
            }

            var response = MapToResponse(category);
            return Ok(ApiResponse<CategoryResponse>.SuccessResponse(
                response,
                "Category retrieved successfully"));
        }

        /// <summary>
        /// POST /api/categories - Create new category
        /// </summary>
        /// <param name="request">Category creation data</param>
        /// <returns>Created category</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public IActionResult CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    errors));
            }

            // Validate ParentCategoryId if provided
            if (request.ParentCategoryId.HasValue)
            {
                var parentCategory = _categoryService.GetCategoryById(request.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Parent category does not exist"));
                }
            }

            var categoryModel = new CategoryModel
            {
                CategoryName = request.CategoryName,
                CategoryDesciption = request.CategoryDesciption,
                ParentCategoryId = request.ParentCategoryId,
                IsActive = request.IsActive ?? true
            };

            var createdCategory = _categoryService.CreateCategory(categoryModel);
            var response = MapToResponse(createdCategory);

            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = response.CategoryId },
                ApiResponse<CategoryResponse>.SuccessResponse(
                    response,
                    "Category created successfully"));
        }

        /// <summary>
        /// PUT /api/categories/{id} - Update existing category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <param name="request">Updated category data</param>
        /// <returns>No content on success</returns>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public IActionResult UpdateCategory(short id, [FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Validation failed",
                    errors));
            }

            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Category with ID {id} not found"));
            }

            // Validate ParentCategoryId if provided
            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId.Value == id)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "A category cannot be its own parent"));
                }

                var parentCategory = _categoryService.GetCategoryById(request.ParentCategoryId.Value);
                if (parentCategory == null)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Parent category does not exist"));
                }
            }

            var updateModel = new CategoryModel
            {
                CategoryName = request.CategoryName,
                CategoryDesciption = request.CategoryDesciption,
                ParentCategoryId = request.ParentCategoryId,
                IsActive = request.IsActive ?? true
            };

            _categoryService.UpdateCategory(id, updateModel);

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "Category updated successfully"));
        }

        /// <summary>
        /// DELETE /api/categories/{id} - Delete category
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>No content on success</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public IActionResult DeleteCategory(short id)
        {
            var existingCategory = _categoryService.GetCategoryById(id);
            if (existingCategory == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse(
                    $"Category with ID {id} not found"));
            }

            var result = _categoryService.DeleteCategory(id);
            if (!result)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(
                    "Cannot delete category with existing news articles"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(
                null,
                "Category deleted successfully"));
        }

        /// <summary>
        /// Maps Business Model to Response Model
        /// </summary>
        private CategoryResponse MapToResponse(CategoryModel model)
        {
            return new CategoryResponse
            {
                CategoryId = model.CategoryId,
                CategoryName = model.CategoryName,
                CategoryDesciption = model.CategoryDesciption,
                ParentCategoryId = model.ParentCategoryId,
                ParentCategoryName = model.ParentCategoryName,
                IsActive = model.IsActive
            };
        }
    }
}
