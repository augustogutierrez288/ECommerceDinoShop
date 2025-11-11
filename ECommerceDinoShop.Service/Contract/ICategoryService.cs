using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> List(string search);
        Task<CategoryDTO> Obtain(int id);
        Task<CategoryDTO> Create(CategoryDTO model);
        Task<bool> Update(CategoryDTO model);
        Task<bool> Delete(int id);
    }
}
