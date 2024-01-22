using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Color { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
