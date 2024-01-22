
namespace QLBH_Dion.Models.ViewModels
{
    public class OrdersViewModel : Order
    {

        public List<AccountBuy>? ListAccountBuy { get; set; }
        public string? OrderTypeName { get; set; }
        public string? OrderStatusName { get; set; }
        public string? OrderStatusColor { get; set; }
        public string? AccountName { get; set; }
        public string? ProductName { get; set; }
        public DateTime RegisterOpenTime { get; set; }
        public DateTime RegisterClosedTime { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime ClosedTime { get; set; }
        public string? ProductBrandName { get; set; }
        public string? ProductCategoryName { get; set; }
        public string? ProvinceName { get; set; }
    }
}
