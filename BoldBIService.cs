using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace BoldBI.MCP.Server.Services
{
    public class BoldBIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public BoldBIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> ExportDashboard(string dashboardId, string format, string fileName)
        {
            var baseUrl = _config["BoldBI:BaseUrl"];
            var token = _config["BoldBI:ApiToken"];

            var url = $"{baseUrl}/api/v1/dashboards/{dashboardId}/export";

            var payload = new
            {
                format = format,
                fileName = fileName
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}