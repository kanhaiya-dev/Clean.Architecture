using Azure;
using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Clean.Architecture.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/accounts/")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(long? accountNumber = null)
        {
            try
            {
                object data;
                if (accountNumber.HasValue)
                {
                    data = await _accountService.GetByAccountNumberAsync(accountNumber.Value);
                    if (data == null)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }
                else
                {
                    data = await _accountService.GetAllAccountsAsync();
                }

                return Ok(data);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountRequest request)
        { 
            try
            {
                await _accountService.CreateAccountAsync(request);

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount(AccountRequest account)
        {
            try
            {
                var response = await _accountService.UpdateAccountAsync(account);
                if (response)
                {
                    return StatusCode(StatusCodes.Status201Created);
                }

                return BadRequest("Update account failed.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(long accountNumber)
        {
            try
            {
                var response = await _accountService.DeleteAccountAsync(accountNumber);
                if (response)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }        
    }
}
