using Clean.Architecture.Core.Entities.Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Response
{
    public class ProductResponse
    {
        public List<Product> Products { get; init; }
    }
}
