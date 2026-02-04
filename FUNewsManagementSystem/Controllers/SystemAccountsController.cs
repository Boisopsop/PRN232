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
    [Route("api/v1/system-accounts")]
    [ApiController]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _accountService;

        public SystemAccountsController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string? name, [FromQuery] string? email, [FromQuery] int? role)
        {
            var accounts = _accountService.SearchAccounts(name, email, role);
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(short id)
        {
            var account = _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateSystemAccountRequest accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if email already exists
            if (_accountService.GetAccountByEmail(accountDto.AccountEmail) != null)
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var account = new SystemAccount
            {
                AccountName = accountDto.AccountName,
                AccountEmail = accountDto.AccountEmail,
                AccountRole = accountDto.AccountRole,
                AccountPassword = accountDto.AccountPassword
            };

            _accountService.CreateAccount(account);
            return CreatedAtAction(nameof(Get), new { id = account.AccountId }, account);
        }

        [HttpPut("{id}")]
        public IActionResult Put(short id, [FromBody] UpdateSystemAccountRequest accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAccount = _accountService.GetAccountById(id);
            if (existingAccount == null)
            {
                return NotFound();
            }

            // Check if email is being changed and if new email already exists
            if (existingAccount.AccountEmail != accountDto.AccountEmail)
            {
                if (_accountService.GetAccountByEmail(accountDto.AccountEmail) != null)
                {
                    return BadRequest(new { message = "Email already exists" });
                }
            }

            existingAccount.AccountName = accountDto.AccountName;
            existingAccount.AccountEmail = accountDto.AccountEmail;
            existingAccount.AccountRole = accountDto.AccountRole;
            
            if (!string.IsNullOrEmpty(accountDto.AccountPassword))
            {
                existingAccount.AccountPassword = accountDto.AccountPassword;
            }

            _accountService.UpdateAccount(existingAccount);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(short id)
        {
            var result = _accountService.DeleteAccount(id);
            if (!result)
            {
                return BadRequest(new { message = "Cannot delete account with existing news articles" });
            }
            return NoContent();
        }
    }
}
