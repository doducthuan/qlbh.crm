using QLBH_Dion.Models.ViewModels;

namespace QLBH_Dion.Models.ViewModel
{
    public class OrderPaging
    {
        public List<OrderSort> data { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int totalPage { get; set; }
    }
}
