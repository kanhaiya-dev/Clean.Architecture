using Clean.Architecture.Core.Interfaces;
using Clean.Architecture.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Infrastructure.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        public UnitOfWorkRepository(IAccountRepository accountRepository)
        {
            AccountRepository = accountRepository;
        }

       public IAccountRepository AccountRepository { get; set; }
    }
}
