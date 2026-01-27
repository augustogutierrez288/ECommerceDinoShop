using ECommerceDinoShop.Model;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> Register(Order model);
    }
}
