using Clean.Architecture.Core.Common.Request;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Accounts.Commands.Delete
{
    public class DeleteAccountCommand : IRequest<bool>
    {
        public long AccountNumber { get; set; }
    }
}
