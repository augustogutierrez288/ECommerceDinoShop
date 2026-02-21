using ECommerceDinoShop.DTO;
using ECommerceDinoShop.DTO.Shipping;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IShippingService
    {
        Task<ResponseDTO<List<ShippingOptionDTO>>> QuoteShipping(string zipCode);
    }
}
