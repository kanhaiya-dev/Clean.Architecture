using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Products.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/products/")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var query = new GetAllProductQuery();
                var content = await _mediator.Send(query);

                if (content == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent);
                }

                List<Product> products = content.Products.ToList();
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
