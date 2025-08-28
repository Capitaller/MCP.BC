using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MCP.BusinessCentral.Infrastructure
{
    public class Client : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _username;
        private readonly string _password;
        private bool _disposed = false;

        public Client(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _baseUrl = configuration["BusinessCentral:BaseUrl"] 
                ?? throw new ArgumentNullException(nameof(configuration), "BusinessCentral:BaseUrl configuration is required");
            _username = configuration["BusinessCentral:Username"] 
                ?? throw new ArgumentNullException(nameof(configuration), "BusinessCentral:Username configuration is required");
            _password = configuration["BusinessCentral:Password"] 
                ?? throw new ArgumentNullException(nameof(configuration), "BusinessCentral:Password configuration is required");
            
            SetupAuthentication();
        }

        private void SetupAuthentication()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<string> GetAsync(string endpoint = "")
        {
            var url = string.IsNullOrWhiteSpace(endpoint) ? _baseUrl : $"{_baseUrl}/{endpoint}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}
