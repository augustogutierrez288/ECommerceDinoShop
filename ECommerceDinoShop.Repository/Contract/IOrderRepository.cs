using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Model;

namespace ECommerceDinoShop.Repository.Contract
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<Order>> GetAllOrdersWithDetails();
        Task<DashboardAnalyticsDTO> GetAnalyticsAsync();
        Task<Order> Register(Order model);
    }
}
