using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("Resume")]
        public IActionResult Resume()
        {
            var response = new ResponseDTO<DashboardDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = _dashboardService.Resume();
            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet("Analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            var response = new ResponseDTO<DashboardAnalyticsDTO>();
            try
            {
                response.Result = await _dashboardService.GetAnalytics();
                response.IsCorrect = true;
            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }
    }
}
