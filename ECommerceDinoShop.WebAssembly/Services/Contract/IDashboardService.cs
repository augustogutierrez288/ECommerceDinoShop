using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Services.Contract
{
    public interface IDashboardService
    {
        Task<ResponseDTO<DashboardDTO>> Resume();
        Task<ResponseDTO<DashboardAnalyticsDTO>> GetAnalytics();
    }
}
