using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IOrderService
    {
        Task<OrderDTO> Register(OrderDTO modelo);
    }
}
