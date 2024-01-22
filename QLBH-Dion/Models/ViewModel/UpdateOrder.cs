namespace QLBH_Dion.Models.ViewModel
{
    public class UpdateOrder
    {
        public int Id { get; set; } 
        public int OrderStatusId { get; set; }
        public string? Note { get; set; }
        public long? Price { get; set; }
    }
}
