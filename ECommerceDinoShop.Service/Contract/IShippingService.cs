using ECommerceDinoShop.DTO.Shipping;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IShippingService
    {
        Task<List<ShippingOptionDTO>> QuoteShipping(string zipCode);
    }
}
