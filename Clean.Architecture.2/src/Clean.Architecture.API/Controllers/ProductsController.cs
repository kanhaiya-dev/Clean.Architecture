using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Common.Utility;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.API.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/products/")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var content = await _productService.GetProductsAsync();
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
