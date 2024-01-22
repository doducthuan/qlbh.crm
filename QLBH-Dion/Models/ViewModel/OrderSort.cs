namespace QLBH_Dion.Models.ViewModel
{
    public class OrderSort
    {
        public int Id { get; set; }
        //Trạng thái Order
        public int orderStatusId { get; set; }
        public string orderStatusName { get; set; }
        public string Note {  get; set; }
        //Tên biển
        public string ProductName { get; set; }
        //Loại biển
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        //Tỉnh thành phố
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        //Loại xe
        public int ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
        //Giá trần
        public long? OrderPrice { get; set; }
        public DateTime BuyTime { get; set; }
        //Ngày đăng order
        public DateTime CreatedTime { get; set; }
        public long? FinalPrice { get; set; } 
    }
}
