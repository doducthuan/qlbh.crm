namespace QLBH_Dion.Services.Interfaces
{
    public interface ITranslateProductService
    {
        Task<string> GetHtmlContentAsync(string url);
        string HandleDoubleChar(string name);
    }
}
