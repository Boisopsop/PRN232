using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Tag;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tagService.GetAllAsync();
            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get tag by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _tagService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.FailureResult("Tag not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Create new tag
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
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
                var result = await _tagService.CreateAsync(request);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.TagID },
                    ApiResponse<object>.SuccessResult(result, "Tag created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Update tag
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagRequest request)
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
                var result = await _tagService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.SuccessResult(result, "Tag updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Delete tag
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "0,1")] // Admin and Staff
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _tagService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<object>.FailureResult("Tag not found"));
                }

                return Ok(new ApiResponse<object> { Success = true, Message = "Tag deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
