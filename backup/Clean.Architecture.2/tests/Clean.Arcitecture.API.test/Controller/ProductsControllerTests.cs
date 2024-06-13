using Xunit;
using Moq;
using System.Threading.Tasks;
using Clean.Architecture.Core.Services.Interfaces;
using Clean.Architecture.API.Controllers;
using Clean.Architecture.Core.Common.Response;
using Microsoft.AspNetCore.Mvc;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Common.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Clean.Arcitecture.API.test.Controller
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockProductService.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WhenProductsFound()
        {
            // Arrange
            var productResponse = new ProductResponse
            {
                Products = new List<Product>
                {
                    new Product { Id = 1, Title = "Product1", Price = 10, Description = "Description1" },
                    new Product { Id = 2, Title = "Product2", Price = 20, Description = "Description2" }
                }
            };
            _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync(productResponse);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.NotNull(returnedProducts);
            Assert.Equal(productResponse.Products.Count, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProducts_ShouldReturnBadRequest_WhenProductsNotFound()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductsAsync()).ReturnsAsync((ProductResponse)null);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductsAsync()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
