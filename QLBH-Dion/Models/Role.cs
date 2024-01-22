using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
            RoleRights = new HashSet<RoleRight>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
