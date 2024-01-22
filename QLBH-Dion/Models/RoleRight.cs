using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class RoleRight
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int RightsId { get; set; }
        public int Active { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Right Rights { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
