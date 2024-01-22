namespace QLBH_Dion.Models.ViewModel
{
    public class OrderStatusViewModel
    {
        public int TotalNoProcess { get; set; } = 0;
        public int TotalApproved { get; set; } = 0;
        public int TotalDeposited { get; set; } = 0;
        public int TotalPurchaed { get; set; } = 0;
        public int TotalNoPurchaed { get; set; } = 0;
        public int TotalCancel { get; set; } = 0;
    }
}
