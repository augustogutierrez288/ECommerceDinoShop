//using Microsoft.AspNetCore.Mvc;
//using ECommerceDinoShop.DTO;
//using MercadoPago.Client.Common;
//using MercadoPago.Client.Preference;
//using MercadoPago.Config;
//using MercadoPago.Resource.Preference;

//namespace ECommerceDinoShop.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartController : ControllerBase
//    {
//        private readonly ILogger<CartController> _logger;
//        private readonly IConfiguration _configuration;

//        public CartController(ILogger<CartController> logger, IConfiguration configuration)
//        {
//            _logger = logger;
//            _configuration = configuration;
//        }

//        [HttpPost]
//        public async Task<IActionResult> PayProducts(SendPaymentDTO model)
//        {
//            if (model.IdentificationType is null)
//            {
//                model.IdentificationType = "DNI";
//            }

//            string accessToken = ObtainAccessToken();

//            MercadoPagoConfig.AccessToken = accessToken;
            

//            // Aquí deberías recibir la lista de productos desde el frontend
//            if (model.Cart == null || model.Cart.Count == 0)
//            {
//                return BadRequest("El carrito está vacío.");
//            }

//            var items = new List<PreferenceItemRequest>();

//            foreach (var item in model.Cart)
//            {
//                items.Add(new PreferenceItemRequest
//                {
                   
//                    Title = item.Product.Name,
//                    Quantity = item.Quantity,
//                    CurrencyId = "ARS",
//                    UnitPrice = (item.Product.SalePrice != 0 && item.Product.SalePrice < item.Product.Price) ? item.Product.SalePrice : item.Product.Price

//                    //Id = item.Product.IdProduct.ToString(),
//                    //Title = item.Product.Name,
//                    //CurrencyId = "ARS",
//                    //PictureUrl = item.Product.ImageUrl,
//                    //Description = item.Product.Description,
//                    //CategoryId = item.Product.IdCategory.ToString(),
//                    //Quantity = item.Quantity,
//                    //UnitPrice = (item.Product.SalePrice != 0 && item.Product.SalePrice < item.Product.Price) ? item.Product.SalePrice : item.Product.Price
//                });
//            }
            

//            var request = new PreferenceRequest
//            {
//                Items = items, //OK

//                //*Aqui estara la informacion del usuario
//                Payer = new PreferencePayerRequest
//                {
//                    Name = model.Email,
//                    Surname = model.Email.ToUpper(),
//                    Email = model.Email,
//                    Identification = new IdentificationRequest
//                    {
//                        Type = model.IdentificationType,
//                        Number = model.IdentificationNumber
//                    },
//                    Address = new AddressRequest
//                    {
//                        StreetName = model.StreetName,
//                        StreetNumber = Convert.ToInt32(model.StreetNumber),
//                        ZipCode = model.ZipCode
//                    },
//                    Phone = new PhoneRequest
//                    {
//                        AreaCode = model.PhoneAreaCode,
//                        Number = model.PhoneNumber
//                    }
//                },

//                BackUrls = new PreferenceBackUrlsRequest
//                {
//                    Success = "https://localhost:7183/cart/aprobado",
//                    Failure = "https://localhost:7183/cart/fail",
//                    Pending = "https://localhost:7183/cart/pendiente"
//                }, //OK
//                AutoReturn = "approved",

//                PaymentMethods = new PreferencePaymentMethodsRequest
//                {
//                    ExcludedPaymentMethods = [],
//                    ExcludedPaymentTypes = [],
//                    Installments = 24
//                },

//                StatementDescriptor = "Mayorista Dino Shop",
//                ExternalReference = $"Referencia_{Guid.NewGuid().ToString()}",
//                Expires = true,
//                ExpirationDateFrom = DateTime.Now,
//                ExpirationDateTo = DateTime.Now.AddMinutes(10)
//            };



//            var client = new PreferenceClient();
//            Preference preference = await client.CreateAsync(request);

//            // Devuelve la URL y el ID de la preferencia al frontend
//            return Ok(new { url = preference.SandboxInitPoint, preferenceId = preference.Id });
//        }


//        #region Data of setting for mercadopago

//        private string ObtainAccessToken()
//        {
//            var token = _configuration.GetValue<string>("MercadoPago:AccessToken");
//            return token ?? string.Empty;
//        }

//        private string ObtainPublicKey()
//        {
//            var token = _configuration.GetValue<string>("MercadoPago:PublicKey");
//            return token ?? string.Empty;
//        }
//        #endregion
//    }
//}
