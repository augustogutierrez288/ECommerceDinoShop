using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> ListAsync();
        Task<OrderDTO> Register(OrderDTO modelo);
    }
}
