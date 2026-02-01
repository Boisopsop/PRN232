using Microsoft.AspNetCore.Mvc;
using PRN232.FUNewsManagementSystem.Repo.Models;
using PRN232.FUNewsManagementSystem.Services.Services.IService;

namespace PRN232.FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountsController(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemAccount>>> GetSystemAccounts()
        {
            var accounts = await _systemAccountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SystemAccount>> GetSystemAccount(int id)
        {
            var account = await _systemAccountService.GetAccountByIdAsync(id);

            if (account == null)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<SystemAccount>> CreateSystemAccount(SystemAccount account)
        {
            var result = await _systemAccountService.CreateAccountAsync(account);

            if (!result)
            {
                return BadRequest(new { message = "Failed to create account. Email may already exist." });
            }

            return CreatedAtAction(nameof(GetSystemAccount), new { id = account.AccountId }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSystemAccount(int id, SystemAccount account)
        {
            if (id != account.AccountId)
            {
                return BadRequest(new { message = "Account ID mismatch." });
            }

            var result = await _systemAccountService.UpdateAccountAsync(account);

            if (!result)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSystemAccount(int id)
        {
            var result = await _systemAccountService.DeleteAccountAsync(id);

            if (!result)
            {
                return NotFound(new { message = $"Account with ID {id} not found." });
            }

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SystemAccount>>> SearchAccounts(
            [FromQuery] string? email,
            [FromQuery] string? name,
            [FromQuery] int? role)
        {
            var accounts = await _systemAccountService.GetAllAccountsAsync();

            if (!string.IsNullOrEmpty(email))
            {
                accounts = accounts.Where(a => a.AccountEmail != null &&
                    a.AccountEmail.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(name))
            {
                accounts = accounts.Where(a => a.AccountName != null &&
                    a.AccountName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (role.HasValue)
            {
                accounts = accounts.Where(a => a.AccountRole == role.Value).ToList();
            }

            return Ok(accounts);
        }
    }
}
