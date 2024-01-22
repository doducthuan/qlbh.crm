using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class OrderAccountPaymentStatus
    {
        public OrderAccountPaymentStatus()
        {
            OrderAccounts = new HashSet<OrderAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual ICollection<OrderAccount> OrderAccounts { get; set; }
    }
}
