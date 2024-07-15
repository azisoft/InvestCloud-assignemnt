using InvestCloud.Extensions;
using InvestCloud.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace InvestCloud
{
    internal class ApiHelper
    {
        private const string _url = "https://recruitment-test.investcloud.com";
        private const string _init = "api/numbers/init/{0}";      // api/numbers/init/{size}
        private const string _getData = "api/numbers/{0}/{1}/{2}";   // api/numbers/{dataset}/{type}/{idx}
        private const string _validateData = "api/numbers/validate";   // api/numbers/validate

        public async Task<bool> Initialize(int size)
        {
            string requestUrl = $"{_url}/{string.Format(_init, size)}";
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var initialize = JsonConvert.DeserializeObject<InitializeResponse>(body);
                return initialize.Success;
            }
            return false;
        }

        public async Task<RowResponse> GetRow(string dataset, int row)
        {
            string requestUrl = $"{_url}/{string.Format(_getData, dataset, "row", row)}";
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RowResponse>(body);
            }
            return null;
        }

        public async Task<string> Validate(byte[] data)
        {
            var sbody = data.ToPlainString();
            var body = new StringContent(sbody, Encoding.UTF8, "application/json");
            string requestUrl = $"{_url}/{_validateData}";
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await httpClient.PostAsync(requestUrl, body);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var resp = JsonConvert.DeserializeObject<ValidationResponse>(result);
                return resp.Value;
            }
            return "Validation API failed to respond.";
        }
    }
}
