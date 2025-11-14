using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResponseDTO<SesionDTO>> Authorization(LoginDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("User/Authorization", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<SesionDTO>>();

            return result!;
        }

        public async Task<ResponseDTO<UserDTO>> Create(UserDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("User/Create", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<UserDTO>>();

            return result!;
        }

        public async Task<ResponseDTO<bool>> Delete(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<ResponseDTO<bool>>($"User/Delete/{id}");
        }

        public async Task<ResponseDTO<List<UserDTO>>> List(string role, string search)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<List<UserDTO>>>($"User/List/{role}/{search}");
        }

        public async Task<ResponseDTO<UserDTO>> Obtain(int id)
        {
            return await _httpClient.GetFromJsonAsync<ResponseDTO<UserDTO>>($"User/Obtain/{id}");
        }

        public async Task<ResponseDTO<bool>> Update(UserDTO model)
        {
            var response = await _httpClient.PutAsJsonAsync("User/Update", model);
            var result = await response.Content.ReadFromJsonAsync<ResponseDTO<bool>>();

            return result!;
        }
    }
}
