using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Mapping;
using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;

namespace WebStore9.Services.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly IProductData _productData;
        private readonly ILogger<InCookiesCartService> _logger;
        private readonly string _cartName;

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData, ILogger<InCookiesCartService> logger)
        {
            _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _productData = productData;
            _logger = logger;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore9.Cart{user_name}";
            _logger.LogInformation("Создан куки для пользователя: {0} с именем корзины: {1}", user.Identity.Name, _cartName);
        }

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookies = context.Request.Cookies[_cartName];
                if (cart_cookies is null) {
                    _logger.LogInformation("Куки с именем {0} не найдены. Создание новой корзины.", _cartName);
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCart(cookies, cart_cookies);
                var deserializedCart = JsonConvert.DeserializeObject<Cart>(cart_cookies);
                _logger.LogInformation("Корзина загружена из куки: {0}", cart_cookies);
                return deserializedCart ?? new Cart();
            }
            set
            {
                var serialized = JsonConvert.SerializeObject(value);
                _logger.LogInformation("Обновление корзины. Новое состояние: {0}", serialized);
                ReplaceCart(_httpContextAccessor.HttpContext.Response.Cookies, serialized);
            }
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cart);
            _logger.LogInformation("Кука {0} заменена на: {1}", _cartName, cart);
        }

        public void Add(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            {
                _logger.LogInformation("Добавление товара с id {0} в корзину [{1}]", id, _cartName);
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
            else
            {
                _logger.LogInformation("Увеличение количества товаров с id {0} в корзине [{1}]", id, _cartName);
                item.Quantity++;
            }

            Cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
            {
                _logger.LogInformation("Количество товаров с id {0} уменьшено до {1} в корзине [{2}]", id, item.Quantity, _cartName);
                item.Quantity--;
            }

            if (item.Quantity <= 0)
            {
                _logger.LogInformation("Удаление товара с id {0} из корзины [{1}]", id, _cartName);
                cart.Items.Remove(item);
            }

            Cart = cart;
        }

        public void Remove(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            _logger.LogInformation("Удаление товара с id {0} из корзины [{1}]", id, _cartName);

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            _logger.LogInformation("Очистка корзины [{0}]", _cartName);

            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            _logger.LogInformation("Формирование модели представления для корзины [{0}]", _cartName);

            var products = _productData.GetProducts(new()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            });

            var products_views = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = Cart.Items
                    .Where(i => products_views.ContainsKey(i.ProductId))
                    .Select(i => (products_views[i.ProductId], i.Quantity))
            };
        }
    }
}
