using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IProductService
    {
        Task<List<ProductDTO>> List(string search);
        Task<List<ProductDTO>> Catalog(string category, string search);
        Task<ProductDTO> Obtain(int id);
        Task<ProductDTO> Create(ProductDTO model);
        Task<bool> Update(ProductDTO model);
        Task<bool> Remove(int id);
    }
}
