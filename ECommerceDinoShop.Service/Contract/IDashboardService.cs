using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.Service.Contract
{
    public interface IDashboardService
    {
        DashboardDTO Resume();
        Task<DashboardAnalyticsDTO> GetAnalytics();
    }
}
