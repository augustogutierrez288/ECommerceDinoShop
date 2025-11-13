using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Register/")]
        public async Task<IActionResult> Register([FromBody] OrderDTO model)
        {
            var response = new ResponseDTO<OrderDTO>();

            try
            {
                response.IsCorrect = true;
                response.Result = await _orderService.Register(model);
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
