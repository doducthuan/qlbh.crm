namespace QLBH_Dion.Models.ViewModel
{
    public class OrderPagingRequest
    {
        public string? keyword { get; set; }
        public int? OrderStatusId { get; set; }
        public int? ProvinceId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? ProductBrandId { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? sortBy { get; set; }
        public string? sortType { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
