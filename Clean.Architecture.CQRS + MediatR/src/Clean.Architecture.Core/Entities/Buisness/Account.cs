using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Core.Entities.Buisness
{
    public class Account : BaseEntity
    { 
        public long CustomerId { get; set; }

        [Key]
        public long AccountNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string BranchAddress { get; set; }
    }
}
