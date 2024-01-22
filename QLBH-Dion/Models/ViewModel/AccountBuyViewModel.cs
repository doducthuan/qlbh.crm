namespace QLBH_Dion.Models.ViewModel
{
    public class AccountBuyViewModel : AccountBuy
    {
        public string? AccountStatusName { get; set; }
        public string? OrderPaymentStatusName { get; set; }
        public int OrderPaymentStatusId { get; set; }
        public int OrderAccountStatusId { get; set; }
        public string? OrderAccountStatusName { get; set; }
        public int OrderAccountId { get; set; }
    }
}
