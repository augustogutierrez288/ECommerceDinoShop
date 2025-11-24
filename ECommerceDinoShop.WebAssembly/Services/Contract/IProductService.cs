using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IProductService
    {
        Task<ResponseDTO<List<ProductDTO>>> List(string search);
        Task<ResponseDTO<List<ProductDTO>>> Catalog(string category, string search);
        Task<ResponseDTO<ProductDTO>> Obtain(int id);
        Task<ResponseDTO<ProductDTO>> Create(ProductDTO model);
        Task<ResponseDTO<bool>> Update(ProductDTO model);
        Task<ResponseDTO<bool>> Delete(int id);
    }
}
