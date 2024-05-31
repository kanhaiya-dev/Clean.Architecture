using Clean.Architecture.Core.Common.Request;
using Clean.Architecture.Core.Common.Response;
using Clean.Architecture.Core.Entities.Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Mapper
{
    public static class AccountMapper
    {
        public static Account MapToAccount(AccountRequest request)
        {
            return new Account
            {
                CustomerId = request.CustomerId,
                AccountType = request.AccountType,
                AccountNumber = request.AccountNumber,
                BranchAddress = request.BranchAddress
            };
        }

        public static AccountResponse MapToAccountResponse(Account account)
        {
            return new AccountResponse
            {
                CustomerId = account.CustomerId,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                BranchAddress = account.BranchAddress
            };
        }
    }
}
