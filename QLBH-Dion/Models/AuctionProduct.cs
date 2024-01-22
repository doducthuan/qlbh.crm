using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class AuctionProduct
    {
        public AuctionProduct()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AuctionProductStatusId { get; set; }
        public int? FinalPrice { get; set; }
        public DateTime RegisterOpenTime { get; set; }
        public DateTime RegisterClosedTime { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ClosedTime { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }

        public virtual AuctionProductStatus AuctionProductStatus { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
