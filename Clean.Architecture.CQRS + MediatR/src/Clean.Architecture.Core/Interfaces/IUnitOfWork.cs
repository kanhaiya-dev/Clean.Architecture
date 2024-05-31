using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
    }
}
