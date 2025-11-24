using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Services.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<OrderDTO>> Register(OrderDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Order/Register", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<OrderDTO>>();

            return result!;
        }
    }
}
