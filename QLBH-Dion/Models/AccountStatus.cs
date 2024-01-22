using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class AccountStatus
    {
        public AccountStatus()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
