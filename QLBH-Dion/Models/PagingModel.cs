namespace QLBH_Dion.Models
{
    public class PagingModel
    {
        public int Id { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchAll { get; set; }
    }
}
