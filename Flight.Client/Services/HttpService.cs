using Flight.Client.Lib.Infra;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Flight.Client.Services
{
    public class HttpService:IHttpService
    {
        private readonly string baseUrlAddress;
        public HttpService()
        {
            baseUrlAddress = "http://localhost:55726/api";
        }

        public async Task<R> PostAsync<T, R>(string url, T payload)
        {
            try
            {
                R result = default;
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var paylodInfo = Task.Run(() => JsonConvert.SerializeObject(payload)).Result;
                    var jsonContent = new StringContent(paylodInfo,
                                        Encoding.UTF8,
                                        "application/json");

                    var response = await httpClient.PostAsync($"{baseUrlAddress}{url}", jsonContent);
                    var responseResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<R>(responseResult);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
