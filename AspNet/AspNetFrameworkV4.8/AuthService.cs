using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetFrameworkV4._8.Models;
using Newtonsoft.Json;

namespace AspNetFrameworkV4._8.Controllers
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService()
        {
            // Configurar para ignorar validación de certificados
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            //_httpClient = new HttpClient() 
            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7126/api/")
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            var jsonContent = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Login/Login", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<AuthResponse>(responseContent);

            return tokenResponse.Token;
        }
    }
}