using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Contract
{
    public interface ICartService
    {
        event Action ShowItems;

        int QuantityProducts();
        Task AddCart(CartDTO model);
        Task DeleteCart(int idProduct);
        Task<List<CartDTO>> ReturnCart();
        Task ClearCart();
    }
}
