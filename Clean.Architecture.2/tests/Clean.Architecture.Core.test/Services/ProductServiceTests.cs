using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services.Implementation;
using Xunit;
using Moq;
using Clean.Architecture.Core.Entities.Buisness;

namespace Clean.Architecture.Core.test.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsProductResponse()
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

            _mockProductRepository.Setup(repo => repo.GetProductsAsync())
                .ReturnsAsync(productResponse);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productResponse, result);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnNull_WhenNoResultFound()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetProductsAsync())
                .ReturnsAsync((ProductResponse?)null);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnException_WhenExceptionThrown()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetProductsAsync())
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _productService.GetProductsAsync());

            //Assert
            Assert.Equal("Test Exception", exception.Message);
        }
    }
}

