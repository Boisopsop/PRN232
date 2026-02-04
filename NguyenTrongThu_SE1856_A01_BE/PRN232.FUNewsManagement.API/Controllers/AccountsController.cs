using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagement.Models.Request.Account;
using PRN232.FUNewsManagement.Models.Response.Common;
using PRN232.FUNewsManagement.Services.Interfaces;

namespace PRN232.FUNewsManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "0")] // Admin only
    public class AccountsController : ControllerBase
    {
        private readonly ISystemAccountService _accountService;

        public AccountsController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Get paginated list of accounts with filtering, sorting, and searching
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? role = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool isDescending = false)
        {
            var result = await _accountService.GetPagedAsync(
                page,
                pageSize,
                searchTerm,
                role,
                sortBy,
                isDescending);

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Get account by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(short id)
        {
            var result = await _accountService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.FailureResult("Account not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(result, "Retrieved successfully"));
        }

        /// <summary>
        /// Create new account
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
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
                var result = await _accountService.CreateAsync(request);
                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.AccountID },
                    ApiResponse<object>.SuccessResult(result, "Account created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Update account
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(short id, [FromBody] UpdateAccountRequest request)
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
                var result = await _accountService.UpdateAsync(id, request);
                return Ok(ApiResponse<object>.SuccessResult(result, "Account updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ApiResponse<object>.FailureResult(ex.Message));
            }
        }

        /// <summary>
        /// Delete account
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                var result = await _accountService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(ApiResponse<object>.FailureResult("Account not found"));
                }

                return Ok(new ApiResponse<object> { Success = true, Message = "Account deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<object>.FailureResult(ex.Message));
            }
        }
    }
}
