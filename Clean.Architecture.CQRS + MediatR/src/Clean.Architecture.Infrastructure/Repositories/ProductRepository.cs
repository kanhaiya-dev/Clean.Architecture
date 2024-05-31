using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Data;
using Clean.Architecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ProductResponse> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("DummyJSON");
            var content = await client.GetFromJsonAsync<ProductResponse>($"/products");
            return content;
        }
    }
}
