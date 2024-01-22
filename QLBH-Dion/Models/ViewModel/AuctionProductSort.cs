namespace QLBH_Dion.Models.ViewModel
{
    public class AuctionProductSort
    {
        public int Id { get; set; }
        //Biển số
        public int productId { get; set; }
        public string productName { get; set; }
        //public string Note { get; set; }
        //Tên biển
        //public string ProductName { get; set; }
        //Loại biển
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        //Tỉnh thành phố
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        //Loại xe
        public int ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        //Thời gian đấu giá
        public DateTime OpenTime { get; set; }
        public DateTime ClosedTime { get; set; }
        //Thời gian kết thúc đăng ký
        public DateTime registerClosedTime { get; set; }
        //Trạng thái tài sản
        public int productStatusId { get; set; }
        public string productStatusName { get; set;}
        //Giá trần
        //public long? OrderPrice { get; set; }
        //public DateTime BuyTime { get; set; }
        ////Ngày đăng order
        //public DateTime CreatedTime { get; set; }
        //public long? FinalPrice { get; set; }
    }
}
