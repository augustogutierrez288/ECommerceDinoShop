using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface ICartService
    {
        event Action ShowItems;

        int QuantityProducts();
        Task AddCart(CartDTO model);
        Task DeleteCart(int idProduct);
        Task<List<CartDTO>> ReturnCart();
        Task ClearCart();
        Task<string?> PayProductsAsync(SendPaymentDTO model);
    }
}
