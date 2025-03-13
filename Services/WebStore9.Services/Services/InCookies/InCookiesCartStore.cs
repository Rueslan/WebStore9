using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InCookies
{
    public class InCookiesCartStore : ICartStore
    {
        private readonly HttpContextAccessor _httpContextAccessor;
        private readonly ILogger<InCookiesCartStore> _logger;
        private readonly string _cartName;

        public Cart cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookies = context.Request.Cookies[_cartName];
                if (cart_cookies is null)
                {
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

        public InCookiesCartStore(IHttpContextAccessor httpContextAccessor, ILogger<InCookiesCartStore> logger)
        {
            _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _logger = logger;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore9.Cart{user_name}";
            _logger.LogInformation("Создан куки для пользователя: {0} с именем корзины: {1}", user.Identity.Name, _cartName);
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cart);
            _logger.LogInformation("Кука {0} заменена на: {1}", _cartName, cart);
        }
    }
}
