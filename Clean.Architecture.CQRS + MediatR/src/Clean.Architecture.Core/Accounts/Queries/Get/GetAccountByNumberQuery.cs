using Clean.Architecture.Core.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Accounts.Queries.Get
{
    public class GetAccountByNumberQuery : IRequest<AccountResponse>
    {
        public long AccountNumber { get; set; }
    }
}
