using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Services.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<CategoryDTO>> Create(CategoryDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Category/Create", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<CategoryDTO>>();

            return result!;
        }

        public async Task<ResponseDTO<bool>> Delete(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"Category/Delete/{id}");
        }

        public async Task<ResponseDTO<List<CategoryDTO>>> List(string search)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<List<CategoryDTO>>>($"Category/List/{search}");
        }

        public async Task<ResponseDTO<CategoryDTO>> Obtain(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<CategoryDTO>>($"Category/Obtain/{id}");
        }

        public async Task<ResponseDTO<bool>> Update(CategoryDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("Category/Update", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();

            return result!;
        }
    }
}
