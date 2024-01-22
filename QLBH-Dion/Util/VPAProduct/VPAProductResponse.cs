namespace QLBH_Dion.Util.VPAProduct
{
    public class VPAProductResponse : VPABaseResponse
    {
        public List<Product> data { get; set; }
        public int total { get; set; }
        public string current_page { get; set; }
    }
    public class Product
    {
        public string id { get; set; }
        public string bks { get; set; }
        public string ngay_dau_gia { get; set; }
        public bool du_kien_dau_gia { get; set; }
        public tinh tinh { get; set; }
        public loai_bien loai_bien { get; set; }
        public loai_xe loai_xe { get; set; }
        public string auctionStartTime { get; set; }
        public string auctionEndTime { get; set; }
        public string registerFromTime { get; set; }
        public string registerToTime { get; set; }

    }
    public class tinh
    {
        public int id { get; set; }
        public string ma { get; set; }
        public string ten { get; set; }

    }
    public class loai_bien
    {
        public int id { get; set; }
        public string ten { get; set; }
    }
    public class loai_xe
    {
        public int id { get; set; }
        public string ma { get; set; }
        public string noi_dung { get; set; }
    }
}
