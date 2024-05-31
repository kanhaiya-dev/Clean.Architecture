using Clean.Architecture.Core.Accounts.Queries.GetAll;
using Clean.Architecture.Core.Common.Mapper;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Data;
using Clean.Architecture.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Products.Queries.GetAll
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public GetAllProductQueryHandler() { }
        public async Task<ProductResponse> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductsAsync();
            
        }
    }
}
