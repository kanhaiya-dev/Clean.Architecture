using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Architecture.Core.Common.Request
{
    public class AccountRequest
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long AccountNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string BranchAddress { get; set; }
    }
}
