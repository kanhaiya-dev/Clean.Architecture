using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductResponse> GetProductsAsync();
    }
}
