using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Accounts.Commands.Create
{
    public class CreateAccountCommand : IRequest
    {
        public AccountRequest AccountRequest { get; set; }
    }
}
