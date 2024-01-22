namespace QLBH_Dion.Models.ViewModel
{
    public class FillterExportOrderExcel
    {
        public string? Id { get; set; } // max order
        public string? ProductName { get; set; } // biển số
        public string? Price { get; set; } // giá trần
        public string? RegisterClosedTime { get; set; } // thời gian kết thúc đăng ký
        public string? AuctionTime { get; set; } // thời gian đấu giá
        public string? AccountName { get; set; }  // fillter : người yêu cầu
        public string[] OrderStatusId { get; set; }  // trang thai yeu cau
        public string? CreatedTime { get; set; } // ngay tao
    }
}
