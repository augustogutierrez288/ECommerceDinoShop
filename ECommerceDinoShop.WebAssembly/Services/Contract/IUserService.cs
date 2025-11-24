using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IUserService
    {
        Task<ResponseDTO<List<UserDTO>>> List(string role, string search);
        Task<ResponseDTO<UserDTO>> Obtain(int id);
        Task<ResponseDTO<SesionDTO>> Authorization(LoginDTO model);
        Task<ResponseDTO<UserDTO>> Create(UserDTO model);
        Task<ResponseDTO<bool>> Update(UserDTO model);
        Task<ResponseDTO<bool>> Delete(int id);
    }
}
