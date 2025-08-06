using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using StreamFlixAPI.DTOs;

namespace StreamFlixAPI.Services
{
    public class GeoLocationService
    {
        private readonly HttpClient _httpClient;

        public GeoLocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GeoInfo> GetLocationAsync(string ip)
        {
            var url = $"http://ip-api.com/json/{ip}";
            return await _httpClient.GetFromJsonAsync<GeoInfo>(url);
        }
    }
}
