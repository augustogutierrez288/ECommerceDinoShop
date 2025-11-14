using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Contract
{
    public interface ICategoryService
    {
        Task<ResponseDTO<List<CategoryDTO>>> List(string search);
        Task<ResponseDTO<CategoryDTO>> Obtain(int id);
        Task<ResponseDTO<CategoryDTO>> Create(CategoryDTO model);
        Task<ResponseDTO<bool>> Update(CategoryDTO model);
        Task<ResponseDTO<bool>> Delete(int id);
    }
}
