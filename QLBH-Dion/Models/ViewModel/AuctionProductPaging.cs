namespace QLBH_Dion.Models.ViewModel
{
    public class AuctionProductPaging
    {
        public List<AuctionProductSort> data { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
    }
}
