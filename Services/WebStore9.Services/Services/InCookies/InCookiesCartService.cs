﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Mapping;
using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;

namespace WebStore9.Services.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly ICartStore _cartStore;
        private readonly IProductData _productData;
        private readonly ILogger<InCookiesCartService> _logger;
        private readonly string _cartName;

        public InCookiesCartService(ICartStore cartStore,IHttpContextAccessor httpContextAccessor, IProductData productData, ILogger<InCookiesCartService> logger)
        {
            _cartStore = cartStore;
            _productData = productData;
            _logger = logger;

            var user = httpContextAccessor.HttpContext!.User;
            var user_name = user.Identity.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _cartName = $"WebStore9.Cart{user_name}";
            _logger.LogInformation("Создан куки для пользователя: {0} с именем корзины: {1}", user.Identity.Name, _cartName);
        }

        public void Add(int id)
        {
            var cart = _cartStore.cart;

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

            _cartStore.cart = cart;
        }

        public void Decrement(int id)
        {
            var cart = _cartStore.cart;

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

            _cartStore.cart = cart;
        }

        public void Remove(int id)
        {
            var cart = _cartStore.cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;

            _logger.LogInformation("Удаление товара с id {0} из корзины [{1}]", id, _cartName);

            cart.Items.Remove(item);

            _cartStore.cart = cart;
        }

        public void Clear()
        {
            _logger.LogInformation("Очистка корзины [{0}]", _cartName);

            var cart = _cartStore.cart;
            cart.Items.Clear();
            _cartStore.cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            _logger.LogInformation("Формирование модели представления для корзины [{0}]", _cartName);

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
