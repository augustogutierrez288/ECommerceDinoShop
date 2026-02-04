using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IOrderService
    {
        Task<ResponseDTO<List<OrderDTO>>> List();
        Task<ResponseDTO<OrderDTO>> Register (OrderDTO model);
    }
}
