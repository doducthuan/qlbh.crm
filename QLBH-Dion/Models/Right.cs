using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Right
    {
        public Right()
        {
            RoleRights = new HashSet<RoleRight>();
        }

        public int Id { get; set; }
        public int Active { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<RoleRight> RoleRights { get; set; }
    }
}
