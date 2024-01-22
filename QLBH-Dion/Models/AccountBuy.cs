using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class AccountBuy
    {
        public AccountBuy()
        {
            OrderAccounts = new HashSet<OrderAccount>();
        }

        public int Id { get; set; }
        public int AccountStatusId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Description { get; set; }
        public string? Info { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }

        public virtual ICollection<OrderAccount> OrderAccounts { get; set; }
    }
}
