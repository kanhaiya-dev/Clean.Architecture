using Clean.Architecture.API.Controllers;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Products.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clean.Arcitecture.API.test.Controller
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ProductsController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ProductsController(_mediatorMock.Object);
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
            _mediatorMock.Setup(service => service.Send(It.IsAny<GetAllProductQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productResponse);

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
            _mediatorMock.Setup(service => service.Send(It.IsAny<GetAllProductQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((ProductResponse)null);

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
            _mediatorMock.Setup(service => service.Send(It.IsAny<GetAllProductQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
