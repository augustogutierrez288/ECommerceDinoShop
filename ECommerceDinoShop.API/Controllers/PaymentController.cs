using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public PaymentController(IProductService productService, IOrderService orderService, IConfiguration configuration)
        {
            _productService = productService;
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] SendPaymentDTO model)
        {
            try
            {
                if (model is null) return BadRequest("Datos inválidos");
                if (string.IsNullOrEmpty(model.IdentificationType)) model.IdentificationType = "DNI";

                // 1. Crear items de la preferencia
                var itemsRequest = new List<PreferenceItemRequest>();
                foreach (var item in model.Items)
                {
                    var productDb = await _productService.Obtain(item.ProductId);
                    if (productDb == null) continue;

                    itemsRequest.Add(new PreferenceItemRequest
                    {
                        Id = productDb.IdProduct.ToString(),
                        Title = productDb.Name,
                        Quantity = item.Quantity,
                        CurrencyId = "ARS",
                        Description = productDb.Description,
                        CategoryId = productDb.IdCategory.ToString(),
                        UnitPrice = (productDb.SalePrice != 0 && productDb.SalePrice < productDb.Price) ? productDb.SalePrice : productDb.Price
                    });
                }

                // Ajusta esta URL según tu entorno local o producción
                var clientUrl = "https://localhost:7183";

                var preferenceRequest = new PreferenceRequest
                {
                    Items = itemsRequest,
                    Payer = new PreferencePayerRequest
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                        Identification = new IdentificationRequest { Type = model.IdentificationType, Number = model.IdentificationNumber },
                        Address = new AddressRequest { StreetName = model.StreetName, StreetNumber = int.TryParse(model.StreetNumber, out int sn) ? sn : 0, ZipCode = model.ZipCode },
                        Phone = new PhoneRequest { AreaCode = model.PhoneAreaCode, Number = model.PhoneNumber }
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = $"{clientUrl}/cart/aprobado",
                        Failure = $"{clientUrl}/cart/fail",
                        Pending = $"{clientUrl}/cart/pendiente"
                    },
                    AutoReturn = "approved",
                    ExternalReference = model.IdUser.ToString(),
                    // Asegúrate de que esta URL sea accesible públicamente (ngrok)
                    NotificationUrl = "https://janiya-oxidimetric-wilfred.ngrok-free.dev/api/payment/webhook",
                    StatementDescriptor = "DinoShop"
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(preferenceRequest);

                return Ok(new ResponseDTO<string> { IsCorrect = true, Result = preference.Id, Message = preference.ApiResponse.ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO<string> { IsCorrect = false, Message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] MercadoPagoWebhookDTO notification)
        {
            // === PASO 1: VALIDACIÓN DE SEGURIDAD CON LOGS ===
            Console.WriteLine($"[Webhook] Recibiendo notificación...");

            // Pasamos el ID del body como respaldo por si no viene en la URL
            string bodyId = notification?.Data?.Id ?? notification?.Id.ToString();

            if (!IsValidSignature(notification))
            {
                Console.WriteLine($"[Webhook Error] Firma inválida. Rechazando con 401.");
                return Unauthorized();
            }

            Console.WriteLine($"[Webhook] Firma Aprobada. Procesando datos...");

            try
            {
                if (notification == null) return Ok();

                // Verificar si es un evento de pago
                if (notification.Type == "payment")
                {
                    // Intentamos obtener el ID, ya sea de Data.Id o del Id raíz
                    string idStr = notification.Data?.Id ?? notification.Id.ToString();

                    if (long.TryParse(idStr, out long paymentId))
                    {
                        Console.WriteLine($"[Webhook] Consultando estado del pago ID: {paymentId}");

                        var client = new PaymentClient();
                        var payment = await client.GetAsync(paymentId);

                        Console.WriteLine($"[Webhook] Estado del pago: {payment.Status}");

                        if (payment.Status == PaymentStatus.Approved)
                        {
                            string userIdStr = payment.ExternalReference;
                            Console.WriteLine($"[Webhook] Pago Aprobado. Usuario ID: {userIdStr}");

                            // Lógica de recuperación de items y guardado...
                            List<OrderDetailDTO> detail = new List<OrderDetailDTO>();

                            if (payment.AdditionalInfo?.Items != null)
                            {
                                foreach (var itemMp in payment.AdditionalInfo.Items)
                                {
                                    if (int.TryParse(itemMp.Id, out int productId))
                                    {
                                        var productDb = await _productService.Obtain(productId);
                                        if (productDb == null) continue;

                                        decimal priceProductDb = (productDb.SalePrice != 0 && productDb.SalePrice < productDb.Price) ? productDb.SalePrice.Value : productDb.Price.Value;

                                        // Parseo seguro de cantidad
                                        int quantity = 1;
                                        if (int.TryParse(itemMp.Quantity?.ToString(), out int q)) quantity = q;

                                        detail.Add(new OrderDetailDTO()
                                        {
                                            IdProduct = productDb.IdProduct,
                                            Quantity = quantity,
                                            Total = priceProductDb * quantity
                                        });
                                    }
                                }
                            }

                            if (detail.Count > 0 && int.TryParse(userIdStr, out int userId))
                            {
                                OrderDTO model = new OrderDTO()
                                {
                                    IdUser = userId,
                                    Total = detail.Sum(i => i.Total),
                                    OrderDetails = detail
                                };

                                var result = await _orderService.Register(model);
                                if (result != null)
                                {
                                    Console.WriteLine($"[Exito] Orden {result.IdOrder} registrada en DB.");
                                }
                                else
                                {
                                    Console.WriteLine($"[Error] El servicio de orden devolvió null.");
                                }
                            }
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Webhook Exception] {ex.Message}");
                return Ok();
            }
        }

        private bool IsValidSignature(MercadoPagoWebhookDTO webhookDTO)
        {
            if (HttpContext!.Request.Headers.TryGetValue("x-signature", out var signatureHeader)
         && HttpContext!.Request.Headers.TryGetValue("x-request-id", out var requestIdHeader)
         && signatureHeader.Count != 0
         && requestIdHeader.Count != 0
         && !string.IsNullOrEmpty(_configuration["MercadoPago:WebhookSecret"]))
            {
                const int prefixLength = 3;
                var headerData = signatureHeader.FirstOrDefault()!.Split(',');
                var timeStamp = headerData.FirstOrDefault()![prefixLength..];
                var hash = headerData.LastOrDefault()![prefixLength..];

                var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_configuration["MercadoPago:WebhookSecret"]));//Signature from Webhook configuration
                var manifest = $"id:{webhookDTO.Data.Id};request-id:{requestIdHeader};ts:{timeStamp};";
                var computedHash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(manifest));
                var computedHashString = Convert.ToHexString(computedHash);

                if (!computedHashString.Equals(hash, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                
                return true;
            }

            return false;
        }

    }
}