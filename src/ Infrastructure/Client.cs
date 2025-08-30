using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace MCP.BusinessCentral.Infrastructure
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _username;
        private readonly string _password;

        public Client(IOptionsSnapshot<BusinessCentralOptions> options, IHttpClientFactory httpClientFactory)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            var cfg = options.Value ?? throw new ArgumentNullException(nameof(options.Value));

            _httpClient = httpClientFactory.CreateClient(nameof(Client));
            _baseUrl = cfg.BaseUrl ?? throw new ArgumentNullException(nameof(cfg.BaseUrl));
            _username = cfg.Username ?? throw new ArgumentNullException(nameof(cfg.Username));
            _password = cfg.Password ?? throw new ArgumentNullException(nameof(cfg.Password));

            SetupAuthentication();
        }

        private void SetupAuthentication()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

    public async Task<string> GetAsync(string? endpoint = null)
        {
            var url = string.IsNullOrWhiteSpace(endpoint) ? _baseUrl : $"{_baseUrl}/{endpoint}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
