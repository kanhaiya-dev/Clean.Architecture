using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Clean.Architecture.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/accounts/")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(long? accountNumber = null)
        {
            try
            {
                _logger.LogInformation("GetAccounts called with accountNumber: {accountNumber}", accountNumber);

                object data;
                if (accountNumber.HasValue)
                {
                    data = await _accountService.GetByAccountNumberAsync(accountNumber.Value);
                    if (data == null)
                    {
                        _logger.LogWarning("No account found with accountNumber: {accountNumber}", accountNumber);
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }
                else
                {
                    data = await _accountService.GetAllAccountsAsync();

                    if (data == null)
                    {
                        _logger.LogWarning("Error occured in fetching account details");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
               
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting accounts");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountRequest request)
        {
            try
            {
                _logger.LogInformation("CreateAccount called with request: {@request}", request);
                await _accountService.CreateAccountAsync(request);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an account");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount(AccountRequest account)
        {
            try
            {
                _logger.LogInformation("UpdateAccount called with account: {@account}", account);
                var response = await _accountService.UpdateAccountAsync(account);
                if (response)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                _logger.LogWarning("Update account failed for account: {@account}", account);
                return BadRequest("Update account failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an account");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(long accountNumber)
        {
            try
            {
                _logger.LogInformation("DeleteAccount called with accountNumber: {accountNumber}", accountNumber);
                var response = await _accountService.DeleteAccountAsync(accountNumber);
                if (response)
                {
                    return Ok();
                }

                _logger.LogWarning("Delete account failed for accountNumber: {accountNumber}", accountNumber);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an account");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
