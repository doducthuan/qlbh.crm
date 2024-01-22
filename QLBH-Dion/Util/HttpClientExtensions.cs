using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using QLBH_Dion.Models;

namespace QLBH_Dion.Util
{
    public static class HttpClientExtensions
    {
        public async static Task<TResponse> SendRequestAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest request) where TRequest : class
        {

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            }
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            });
        }

        /// <summary>
        /// Author: Daniel
        /// CreatedTime: 27/07/2023
        /// Description: Get Request
        /// </summary>
        /// <typeparam name="TResponse">Dữ liệu trả về</typeparam>
        /// <param name="httpClient">biến httpClient</param>
        /// <param name="url">API get</param>
        /// <returns>Dữ liệu trả về</returns>
        /// <exception cref="ApplicationException"></exception>
        public async static Task<TResponse> GetRequestAsync<TResponse>(this HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                //throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            }
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

        public async static Task<TResponse> SendDataFormRequestAsync<TRequest, TResponse>(this HttpClient httpClient, string url, TRequest request) where TRequest : class
        {
            var content = new MultipartFormDataContent();
            var propertyInfos = request.GetType().GetProperties();
            foreach (PropertyInfo pInfo in propertyInfos)
            {
                string propertyName = pInfo.Name; //gets the name of the property
                var propertyValue = pInfo.GetValue(request, null).ToString();
                content.Add(new StringContent(propertyValue), propertyName);
            }
            var response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
            }
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        }

    }
}
