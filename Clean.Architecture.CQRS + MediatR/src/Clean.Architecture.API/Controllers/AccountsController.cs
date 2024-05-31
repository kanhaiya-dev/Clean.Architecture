using Clean.Architecture.Core.Accounts.Commands.Create;
using Clean.Architecture.Core.Accounts.Commands.Delete;
using Clean.Architecture.Core.Accounts.Commands.Update;
using Clean.Architecture.Core.Accounts.Queries.Get;
using Clean.Architecture.Core.Accounts.Queries.GetAll;
using Clean.Architecture.Core.Common.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/accounts/")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAccounts(long? accountNumber = null)
        {
            try
            {
                object data;
                if (accountNumber.HasValue)
                {
                    var query = new GetAccountByNumberQuery { AccountNumber = (long)accountNumber };
                    data = await _mediator.Send(query);

                    if (data == null)
                    {
                        return StatusCode(StatusCodes.Status204NoContent);
                    }
                }
                else
                {
                    data = await _mediator.Send(new GetAllAccountQuery());
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
                await _mediator.Send(new CreateAccountCommand { AccountRequest = request });

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
                var command = new UpdateAccountCommand { AccountRequest = account };
                var response = await _mediator.Send(command);

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
                var response = await _mediator.Send(new DeleteAccountCommand { AccountNumber = accountNumber });
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
