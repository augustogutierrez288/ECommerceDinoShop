using ECommerceDinoShop.DTO;

namespace ECommerceDinoShop.WebAssembly.Contract
{
    public interface IDashboardService
    {
        Task<ResponseDTO<DashboardDTO>> Resume();
    }
}
