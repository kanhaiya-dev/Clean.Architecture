using Azure.Core;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Infrastructure.Repositories;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

public class ProductRepositoryTests
{
    [Fact]
    public async Task GetProductsAsync_ReturnsSampleProduct()
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

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(productResponse)
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var productRepository = new ProductRepository(httpClientFactoryMock.Object);

        // Act
        var result = await productRepository.GetProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Products.Count);
    }

    [Fact]
    public async Task GetProductsAsync_ThrowsException_WhenHttpClientFails()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var sampleApiService = new ProductRepository(httpClientFactoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => sampleApiService.GetProductsAsync());
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsNull_WhenApiResponseIsEmpty()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var sampleApiService = new ProductRepository(httpClientFactoryMock.Object);

        // Act
        var result = await sampleApiService.GetProductsAsync();

        // Assert
        Assert.Null(result.Products);
    }
}
