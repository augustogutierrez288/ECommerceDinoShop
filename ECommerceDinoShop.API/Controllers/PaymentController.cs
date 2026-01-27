using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService; // Inyectamos el servicio de Ordenes
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

                if (string.IsNullOrEmpty(model.IdentificationType))
                {
                    model.IdentificationType = "DNI";
                }

                // 1. Recuperar productos reales de la DB
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
                        // Precio real desde la DB
                        UnitPrice = (productDb.SalePrice != 0 && productDb.SalePrice < productDb.Price) ? productDb.SalePrice : productDb.Price
                    });
                }

                var clientUrl = "https://localhost:7183";

                // 2. Crear la solicitud
                var preferenceRequest = new PreferenceRequest
                {
                    Items = itemsRequest,
                    Payer = new PreferencePayerRequest
                    {
                        Name = model.Name,
                        Surname = model.Surname,
                        Email = model.Email,
                        Identification = new IdentificationRequest
                        {
                            Type = model.IdentificationType,
                            Number = model.IdentificationNumber
                        },
                        Address = new AddressRequest
                        {
                            StreetName = model.StreetName,
                            StreetNumber = int.TryParse(model.StreetNumber, out int sn) ? sn : 0,
                            ZipCode = model.ZipCode
                        },
                        Phone = new PhoneRequest
                        {
                            AreaCode = model.PhoneAreaCode,
                            Number = model.PhoneNumber
                        }
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = $"{clientUrl}/cart/aprobado",
                        Failure = $"{clientUrl}/cart/fail",
                        Pending = $"{clientUrl}/cart/pendiente"
                    },
                    AutoReturn = "approved",
                    // IMPORTANTE: Pasamos el ID del Usuario en ExternalReference para recuperarlo en el Webhook
                    ExternalReference = model.IdUser,
                    NotificationUrl = "https://janiya-oxidimetric-wilfred.ngrok-free.dev/api/payment/webhook",
                    StatementDescriptor = "Mayorista DinoShop"
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(preferenceRequest);

                return Ok(new ResponseDTO<string> { IsCorrect = true, Result = preference.Id, Message = preference.NotificationUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseDTO<string> { IsCorrect = false, Message = ex.Message });
            }
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] MercadoPagoWebhookDTO notification)
        {
            // 1. VALIDACIÓN DE SEGURIDAD
            if (!IsValidSignature(Request))
            {
                return Unauthorized();
            }

            try
            {
                if (notification == null) return Ok();

                if (notification.type == "payment" && notification.data != null && !string.IsNullOrEmpty(notification.data.id))
                {
                    if (long.TryParse(notification.data.id, out long paymentId))
                    {
                        var client = new PaymentClient();

                        MercadoPago.Resource.Payment.Payment payment = null;

                        // Paso C: Procesamos si obtuvimos el pago y está aprobado
                        if (payment != null && payment.Status == PaymentStatus.Approved)
                        {
                            string userIdStr = payment.ExternalReference;

                            List<OrderDetailDTO> detail = new List<OrderDetailDTO>();

                            if (payment.AdditionalInfo != null && payment.AdditionalInfo.Items != null)
                            {
                                foreach (var itemMp in payment.AdditionalInfo.Items)
                                {
                                    if (int.TryParse(itemMp.Id, out int productId))
                                    {
                                        var productDb = await _productService.Obtain(productId);
                                        if (productDb == null) continue;

                                        decimal priceProductDb = (productDb.SalePrice != 0 && productDb.SalePrice < productDb.Price) ? productDb.SalePrice.Value : productDb.Price.Value;

                                        int quantity = 1;
                                        if (itemMp.Quantity != null) int.TryParse(itemMp.Quantity.ToString(), out quantity);

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
                                    Console.WriteLine($"[Exito] Venta registrada. Orden ID: {result.IdOrder}");
                                }
                            }
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Webhook: {ex.Message}");
                return Ok();
            }
        }

        private bool IsValidSignature(HttpRequest request)
        {
            try
            {
                // 1. CORRECCIÓN: Usar los nombres reales de los Headers
                string xSignature = request.Headers["x-signature"];
                string xRequestId = request.Headers["x-request-id"];

                // 2. CORRECCIÓN: Usar el nombre real del Query Param
                string dataID = request.Query["data.id"];

                // Validaciones básicas
                if (string.IsNullOrEmpty(xSignature) || string.IsNullOrEmpty(xRequestId) || string.IsNullOrEmpty(dataID))
                    return false;

                // B. Separar partes de la firma (ts y v1)
                var parts = xSignature.Split(',');
                string ts = null;
                string hashRecibido = null;

                foreach (var part in parts)
                {
                    var keyValue = part.Split('=', 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        if (key == "ts") ts = value;
                        else if (key == "v1") hashRecibido = value;
                    }
                }

                // C. Obtener tu clave secreta
                string secret = _configuration["MercadoPago:WebhookSecret"];
                if (string.IsNullOrEmpty(secret)) return false;

                // D. Generar el manifiesto
                // NOTA: Aquí sí usamos el formato del template que pide la doc, concatenando las variables
                // Template: id:[data.id];request-id:[x-request-id];ts:[ts];
                string manifest = $"id:{dataID};request-id:{xRequestId};ts:{ts};";

                // E. Calcular HMAC SHA-256
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest));
                    var computedHashString = BitConverter.ToString(computedHash).Replace("-", "").ToLower();

                    // F. Comparar hash calculado vs recibido
                    return computedHashString == hashRecibido;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}