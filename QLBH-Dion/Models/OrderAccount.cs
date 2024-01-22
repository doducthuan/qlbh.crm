using System;
using System.Collections.Generic;

namespace QLBH_Dion.Models
{
    public partial class OrderAccount
    {
        public int Id { get; set; }
        public int AccountBuyId { get; set; }
        public int OrderId { get; set; }
        public int OrderPaymentStatusId { get; set; }
        public int? MaxPrice { get; set; }
        public int? WinPrice { get; set; }
        public int OrderAccountStatusId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Active { get; set; }
        public DateTime CreatedTime { get; set; }
        public string? Search { get; set; }
        public virtual AccountBuy AccountBuy { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual OrderAccountStatus OrderAccountStatus { get; set; } = null!;
        public virtual OrderAccountPaymentStatus OrderPaymentStatus { get; set; } = null!;
    }
}
