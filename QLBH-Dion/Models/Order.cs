using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderAccounts = new HashSet<OrderAccount>();
            OrderUpdateHistories = new HashSet<OrderUpdateHistory>();
        }

        public int Id { get; set; }
        public int AuctionProductId { get; set; }
        public int OrderTypeId { get; set; }
        public int OrderStatusId { get; set; }
        public int AccountId { get; set; }
        public string? Description { get; set; }
        public string? Feedback { get; set; }
        public int? Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }
        public string? Note { get; set; }
        public string? Note2 { get; set; }
        public string? Note3 { get; set; }
        public string? Note4 { get; set; }
        public string? Note5 { get; set; }
        public long? Price { get; set; }
        public long? FinalPrice { get; set; }
        public virtual Account Account { get; set; } = null!;
        public virtual AuctionProduct AuctionProduct { get; set; } = null!;
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual OrderType OrderType { get; set; } = null!;
        public virtual ICollection<OrderAccount> OrderAccounts { get; set; }
        public virtual ICollection<OrderUpdateHistory> OrderUpdateHistories { get; set; }
    }
}
