using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> GetProductsAsync()
        {           
            return await _productRepository.GetProductsAsync();
        }
    }
}
