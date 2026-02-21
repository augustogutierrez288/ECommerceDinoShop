using ECommerceDinoShop.DTO;
using ECommerceDinoShop.DTO.Shipping;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet("Quote/{zipCode}")]
        public async Task<IActionResult> Quote(string zipCode)
        {
            var result = await _shippingService.QuoteShipping(zipCode);
            return Ok(new ResponseDTO<List<ShippingOptionDTO>> { IsCorrect = true, Result = result });
        }
    }
}
