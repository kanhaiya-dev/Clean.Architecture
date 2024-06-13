using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Response
{
    public class AccountResponse
    {
        public long CustomerId { get; set; }

        public long AccountNumber { get; set; }

        public string AccountType { get; set; }

        public string BranchAddress { get; set; }
    }
}
