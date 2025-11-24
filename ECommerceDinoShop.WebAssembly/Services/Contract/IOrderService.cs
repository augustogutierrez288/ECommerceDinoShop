using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IOrderService
    {
        Task<ResponseDTO<OrderDTO>> Register (OrderDTO model);
    }
}
