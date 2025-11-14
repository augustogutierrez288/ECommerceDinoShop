using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Contract
{
    public interface IOrderService
    {
        Task<ResponseDTO<OrderDTO>> Register (OrderDTO model);
    }
}
