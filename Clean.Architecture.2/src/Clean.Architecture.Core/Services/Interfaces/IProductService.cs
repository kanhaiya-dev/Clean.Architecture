using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using Clean.Architecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> GetProductsAsync();
    }
}
