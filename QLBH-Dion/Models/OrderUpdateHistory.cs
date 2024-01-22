using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class OrderUpdateHistory
    {
        public int Id { get; set; }
        public int Active { get; set; }
        public int OrderId { get; set; }
        public int AccountId { get; set; }
        public string? Name { get; set; }
        public string? ObjectOld { get; set; }
        public string? ObjectNew { get; set; }
        public string? Change { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }
        public string? Browser { get; set; }
        public string? Os { get; set; }
        public string? UserAgent { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
