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
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                _logger.LogInformation("GetProducts called");

                var content = await _productService.GetProductsAsync();
                if (content == null)
                {
                    _logger.LogWarning("No products found");
                    return StatusCode(StatusCodes.Status204NoContent);
                }

                List<Product> products = content.Products.ToList();
                _logger.LogInformation("Products retrieved successfully: {ProductCount} items", products.Count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting products");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
