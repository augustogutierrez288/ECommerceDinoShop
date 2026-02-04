using ECommerceDinoShop.Model;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersWithDetails();
        Task<Order> Register(Order model);
    }
}
