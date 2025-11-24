using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Services.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<DashboardDTO>> Resume()
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<DashboardDTO>>($"Dashboard/Resume");
        }
    }
}
