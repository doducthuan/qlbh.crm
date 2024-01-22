namespace QLBH_Dion.Util.Provinces
{
    public interface IProvinceUtil
    {
        public const string PROVINCE_OPEN_API = "https://provinces.open-api.vn/api/?depth=";
        /// <summary>
        /// CreatedDate: 01/11/2023
        /// Author: DungxTT
        /// Description: Get province
        /// </summary>
        /// <param name="depth">
        /// depth=1: only province
        /// depth=2: province, districts 
        /// depth=3: province, districts, wards
        /// </param>
        /// <returns>List provinces</returns>
        Task<List<ProvinceReponse>> GetProvinces(string depth);

    }

    public class ProvinceUtil : IProvinceUtil
    {
        private const string PROVINCE_OPEN_API = "https://provinces.open-api.vn/api/?depth=";
        public async Task<List<ProvinceReponse>> GetProvinces(string depth)
        {
            using HttpClient httpClient = new HttpClient();
            var url = PROVINCE_OPEN_API + depth;
            var response = await httpClient.GetRequestAsync<List<ProvinceReponse>>(url);
            return response;
        }
    }
}
