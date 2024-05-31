using Clean.Architecture.Core.Common.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Accounts.Commands.Update
{
    public class UpdateAccountCommand : IRequest<bool>
    {
        public AccountRequest AccountRequest { get; set; }
    }
}
