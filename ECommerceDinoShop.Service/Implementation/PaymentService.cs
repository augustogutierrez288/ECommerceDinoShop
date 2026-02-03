using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceDinoShop.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public PaymentService(IProductService productService, IOrderService orderService, IConfiguration configuration)
        {
            _productService = productService;
            _orderService = orderService;
            _configuration = configuration;
        }

        public async Task<Preference> CreatePreferenceAsync(SendPaymentDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            if (string.IsNullOrEmpty(model.IdentificationType)) model.IdentificationType = "DNI";

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

            var clientUrl = "https://localhost:7183"; 
            var notificationUrl = "https://janiya-oxidimetric-wilfred.ngrok-free.dev/api/payment/webhook";

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
                NotificationUrl = notificationUrl,
                StatementDescriptor = "DinoShop"
            };

            var client = new PreferenceClient();
            return await client.CreateAsync(preferenceRequest);
        }

        public bool ValidateWebhookSignature(string xSignature, string xRequestId, string queryDataId, string bodyId)
        {
            try
            {
                string dataID = queryDataId;
                if (string.IsNullOrEmpty(dataID))
                {
                    dataID = bodyId;
                }

                if (string.IsNullOrEmpty(xSignature) || string.IsNullOrEmpty(xRequestId) || string.IsNullOrEmpty(dataID))
                    return false;

                var parts = xSignature.Split(',');
                string ts = null;
                string hashRecibido = null;

                foreach (var part in parts)
                {
                    var kv = part.Split('=', 2);
                    if (kv.Length == 2)
                    {
                        var key = kv[0].Trim();
                        var value = kv[1].Trim();
                        if (key == "ts") ts = value;
                        else if (key == "v1") hashRecibido = value;
                    }
                }

                string secret = _configuration["MercadoPago:WebhookSecret"];
                if (string.IsNullOrEmpty(secret)) return false;

                string manifest = $"id:{dataID};request-id:{xRequestId};ts:{ts};";

                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
                {
                    var computedHashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest));
                    var computedHashString = BitConverter.ToString(computedHashBytes).Replace("-", "").ToLower();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ProcessPaymentAsync(long paymentId)
        {
            try
            {
                // Pequeño delay opcional para evitar race condition (404 Not Found inmediato)
                // await Task.Delay(2000); 

                var client = new PaymentClient();
                var payment = await client.GetAsync(paymentId);

                if (payment.Status == PaymentStatus.Approved)
                {
                    string userIdStr = payment.ExternalReference;
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
                        return result != null;
                    }
                }
                return true; // El proceso se ejecutó correctamente (aunque no se guardara orden si no estaba aprobado)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando pago: {ex.Message}");
                return false;
            }
        }
    }
}