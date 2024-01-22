namespace QLBH_Dion.Models.ViewModel
{
    public class AuctionProductPagingRequest
    {
        public string? keyword { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? ProvinceId { get; set; }
        public string? FromRegisterClosedTime {  get; set; }
        public string? ToRegisterClosedTime { get; set; }
        public int? AuctionProductBrandId { get; set; }
        public int? AuctionProductCategoryId { get; set; }
        public int? AuctionProductStatusId { get; set; }
        public int? ProductProvince {  get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string? sortBy { get; set; }
        public string? sortType { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}
