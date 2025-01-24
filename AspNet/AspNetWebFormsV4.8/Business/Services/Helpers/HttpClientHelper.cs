using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AspNetWebFormsV4._8.Business.Services.Helpers
{
    public static class HttpClientHelper
    {
        // Configurar para ignorar validación de certificados en entornos de desarrollo, pruebas
        private static readonly HttpClientHandler _httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, sslPolicyErrors) => true
        };

        private static readonly HttpClient _httpClient = new HttpClient(_httpClientHandler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        //private static readonly HttpClient _httpClient = new HttpClient
        //{
        //    Timeout = TimeSpan.FromSeconds(30)
        //};

        public static async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<T> PostAsync<T>(string url, object data)
        {
            var jsonContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task PutAsync(string url, object data)
        {
            var jsonContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, content);
            response.EnsureSuccessStatusCode();
        }

        public static async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}