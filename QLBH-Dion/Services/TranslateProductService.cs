using MathNet.Numerics.Distributions;
using QLBH_Dion.Services.Interfaces;
using System.Text;

namespace QLBH_Dion.Services
{
    public class TranslateProductService : ITranslateProductService
    {
        public async Task<string> GetHtmlContentAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Gửi yêu cầu GET đến trang web và nhận nội dung HTML
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Kiểm tra xem yêu cầu có thành công hay không (status code 200 là thành công)
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
        }
        public string HandleDoubleChar(string name)
        {
            HashSet<char> kyTuDuocXuatHien = new HashSet<char>();

            string result = "";
            foreach (char kyTu in name)
            {
                if (kyTuDuocXuatHien.Add(kyTu))
                {
                    result += kyTu + "_";
                }
            }
            return result;
        }
    }
}
