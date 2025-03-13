using Microsoft.Extensions.Logging;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Mapping;
using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;

namespace WebStore9.Services.Services
{
    public class CartService : ICartService
    {
        private readonly ICartStore _cartStore;
        private readonly IProductData _productData;
        private readonly ILogger<CartService> _logger;

        public CartService(ICartStore cartStore, IProductData productData, ILogger<CartService> logger)
        {
            _cartStore = cartStore;
            _productData = productData;
            _logger = logger;
        }

        public void Add(int id)
        {
            var cart = _cartStore.cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null)
            {
                _logger.LogInformation("Добавление товара с id {0}", id);
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
            else
            {
                _logger.LogInformation("Увеличение количества товаров с id {0}", id);
                item.Quantity++;
            }

            _cartStore.cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = _cartStore.cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            if (item.Quantity > 0)
            {
                _logger.LogInformation("Количество товаров с id {0} уменьшено до {1}", id, item.Quantity);
                item.Quantity--;
            }

            if (item.Quantity <= 0)
            {
                _logger.LogInformation("Удаление товара с id {0}", id);
                cart.Items.Remove(item);
            }

            _cartStore.cart = cart;
        }

        public void Remove(int id)
        {
            var cart = _cartStore.cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            _logger.LogInformation("Удаление товара с id {0}", id);

            cart.Items.Remove(item);

            _cartStore.cart = cart;
        }

        public void Clear()
        {
            _logger.LogInformation("Очистка корзины");

            var cart = _cartStore.cart;
            cart.Items.Clear();
            _cartStore.cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            _logger.LogInformation("Формирование модели представления для корзины");

            var products = _productData.GetProducts(new()
            {
                Ids = _cartStore.cart.Items.Select(i => i.ProductId).ToArray()
            });

            var products_views = products.ToView().ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = _cartStore.cart.Items
                    .Where(i => products_views.ContainsKey(i.ProductId))
                    .Select(i => (products_views[i.ProductId], i.Quantity))
            };
        }
    }
}
