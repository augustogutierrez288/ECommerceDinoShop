using Blazored.LocalStorage;
using Blazored.Toast.Services;
using ECommerceDinoShop.DTO;
using ECommerceDinoShop.WebAssembly.Services.Contract;
using System.Net.Http.Json;

namespace ECommerceDinoShop.WebAssembly.Services.Implementation
{
    public class CartService : ICartService
    {
        private ILocalStorageService _localStorageService;
        private ISyncLocalStorageService _syncLocalStorageService;
        private IToastService _toastService;
        private readonly HttpClient _httpClient;

        public CartService(
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService,
            IToastService toastService,
            HttpClient httpClient
            )
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _toastService = toastService;
            _httpClient = httpClient;
        }

        public event Action ShowItems;

        public async Task AddCart(CartDTO model)
        {
            try
            {
                var cart = await _localStorageService.GetItemAsync<List<CartDTO>>("cart");
                if (cart == null)
                    cart = new List<CartDTO>();

                var found = cart.FirstOrDefault(c => c.Product.IdProduct == model.Product.IdProduct);

                if (found != null)
                    cart.Remove(found);

                cart.Add(model);
                await _localStorageService.SetItemAsync("cart", cart);

                if (found != null)
                    _toastService.ShowSuccess("Producto fue actualizado en carrito");
                else
                    _toastService.ShowSuccess("Producto fue agregado en carrito");

                ShowItems.Invoke();

            }
            catch
            {
                _toastService.ShowSuccess("No se pudo agregar al carrito");
            }
        }

        public async Task ClearCart()
        {
            await _localStorageService.RemoveItemAsync("cart");
            ShowItems.Invoke();
        }

        public async Task DeleteCart(int idProduct)
        {
            try
            {
                var cart = await _localStorageService.GetItemAsync<List<CartDTO>>("cart");
                if (cart != null)
                {
                    var element = cart.FirstOrDefault(c => c.Product.IdProduct == idProduct);
                    if (element != null)
                    {
                        cart.Remove(element);
                        await _localStorageService.SetItemAsync("cart", cart);
                        ShowItems.Invoke();
                    }

                }
            }
            catch
            {
                _toastService.ShowSuccess("No se pudo eliminar el producto");
            }
        }

        public int QuantityProducts()
        {
            var cart = _syncLocalStorageService.GetItem<List<CartDTO>>("cart");

            if (cart == null || !cart.Any())
                return 0;

            // Sumar las cantidades de cada carrito; Quantity es nullable, se usa GetValueOrDefault()
            return cart.Sum(c => c.Quantity.GetValueOrDefault());
        }

        public async Task<List<CartDTO>> ReturnCart()
        {
            var cart = await _localStorageService.GetItemAsync<List<CartDTO>>("cart");

            if (cart == null)
                cart = new List<CartDTO>();

            return cart;
        }

        public async Task<string?> PayProductsAsync(SendPaymentDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("Cart", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaymentUrlResponse>();
                return result?.url;
            }
            return null;
        }

        public class PaymentUrlResponse
        {
            public string url { get; set; }
        }
    }
}
