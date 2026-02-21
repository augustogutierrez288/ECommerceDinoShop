using ECommerceDinoShop.DTO;
using ECommerceDinoShop.DTO.Shipping;
using ECommerceDinoShop.WebAssembly.Services.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Services.Implementation
{
    public class ShippingService : IShippingService
    {
        private readonly HttpClient _httpClient;

        public ShippingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<List<ShippingOptionDTO>>> QuoteShipping(string zipCode)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<List<ShippingOptionDTO>>>($"Shipping/Quote/{zipCode}");
        }
    }
}
