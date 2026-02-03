using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] SendPaymentDTO model)
        {
            try
            {
                var preference = await _paymentService.CreatePreferenceAsync(model);
                return Ok(new ResponseDTO<string>
                {
                    IsCorrect = true,
                    Result = preference.Id,
                    Message = "Preferencia creada exitosamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO<string> { IsCorrect = false, Message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] MercadoPagoWebhookDTO notification)
        {
            // 1. Obtener Datos de Cabecera y URL
            string xSignature = Request.Headers["x-signature"];
            string xRequestId = Request.Headers["x-request-id"];
            string queryId = Request.Query["data.id"];

            // 2. Obtener ID de respaldo del Body
            string bodyId = notification?.Data?.Id ?? notification?.Id.ToString();

            // 3. Delegar validación de seguridad al servicio
            bool isValid = _paymentService.ValidateWebhookSignature(xSignature, xRequestId, queryId, bodyId);

            if (!isValid)
            {
                Console.WriteLine("[Webhook] Rechazado: Firma inválida.");
                return Unauthorized();
            }

            Console.WriteLine("[Webhook] Firma válida.");

            // 4. Delegar lógica de negocio al servicio
            if (notification?.Type == "payment")
            {
                string idStr = notification.Data?.Id ?? notification.Id.ToString();
                if (long.TryParse(idStr, out long paymentId))
                {
                    await _paymentService.ProcessPaymentAsync(paymentId);
                }
            }

            return Ok();
        }
    }
}