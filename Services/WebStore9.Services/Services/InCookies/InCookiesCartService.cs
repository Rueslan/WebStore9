using Microsoft.AspNetCore.Http;
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
        private readonly string _CartName;

        public InCookiesCartService(IHttpContextAccessor httpContextAccessor, IProductData productData)
        {
            _httpContextAccessor = (HttpContextAccessor)httpContextAccessor;
            _productData = productData;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore9.Cart{user_name}";
        }

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context!.Response.Cookies;

                var cart_cookies = context.Request.Cookies[_CartName];
                if (cart_cookies is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCart(cookies, cart_cookies);
                return JsonConvert.DeserializeObject<Cart>(cart_cookies);
            }
            set => ReplaceCart(_httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cart);
        }

        public void Add(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null)
                cart.Items.Add(new CartItem { ProductId = Id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public void Decrement(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null) return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null) return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public void Clear()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public async Task<CartViewModel> GetViewModelAsync()
        {
            var products = await _productData.GetProductsAsync(new()
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
