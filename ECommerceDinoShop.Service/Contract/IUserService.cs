using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IUserService
    {
        Task<List<UserDTO>> List(string role, string search);
        Task<UserDTO> Obtain(int id);
        Task<SesionDTO> Authorization(LoginDTO model);
        Task<UserDTO> Create(UserDTO model);
        Task<bool> Update(UserDTO model);
        Task<bool> Remove(int id);
    }
}
