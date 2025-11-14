using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Implementation
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<List<ProductDTO>>> Catalog(string category, string search)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<List<ProductDTO>>>($"Product/Catalog/{category}/{search}");
        }

        public async Task<ResponseDTO<ProductDTO>> Create(ProductDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Product/Create", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<ProductDTO>>();

            return result;
        }

        public async Task<ResponseDTO<bool>> Delete(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Product/Delete/{id}");
        }

        public async Task<ResponseDTO<List<ProductDTO>>> List(string search)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<List<ProductDTO>>>($"Product/List/{search}");
        }

        public async Task<ResponseDTO<ProductDTO>> Obtain(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<ProductDTO>>($"Product/Obtain/{id}");
        }

        public async Task<ResponseDTO<bool>> Update(ProductDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Product/Update", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();

            return result!;
        }
    }
}
