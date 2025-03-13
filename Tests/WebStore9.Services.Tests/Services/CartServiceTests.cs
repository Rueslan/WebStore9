using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore9.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;

        [TestInitialize]
        public void TestInitialize()
        {
            _cart = new Cart()
            {
                Items = new List<CartItem>()
                {
                    new(){ProductId = 1, Quantity = 1},
                    new(){ProductId = 2, Quantity = 3}
                }
            };
        }

        [TestMethod]
        public void CartClassItemsCountReturnsCorrectQuantity()
        {
            var cart = _cart;
            var expectedItemsCount = cart.Items.Sum(i => i.Quantity);
            var actualItemsCount = cart.ItemsCount;
            Assert.Equal(expectedItemsCount, actualItemsCount);
        }

        [TestMethod]
        public void CartViewModelReturnsCorrectItemsCount()
        {
            var cartViewModel = new CartViewModel()
            {
                Items = new[]
                {
                    (new ProductViewModel(){Id = 1, Name = "Product 1", Price = 1.5m}, 1),
                    (new ProductViewModel(){Id = 2, Name = "Product 2", Price = 2.5m}, 3)
                }
            };

            var expectedItemsCount = cartViewModel.Items.Sum(i => i.Quantity);
            var actualItemsCount = cartViewModel.ItemsCount;
            Assert.Equal(expectedItemsCount, actualItemsCount);
        }

        [TestMethod]
        public void CartViewModelReturnsCorrectTotalPrice()
        {
            var cartViewModel = new CartViewModel()
            {
                Items = new[]
                {
                    (new ProductViewModel(){Id = 1, Name = "Product 1", Price = 1.5m}, 1),
                    (new ProductViewModel(){Id = 2, Name = "Product 2", Price = 2.5m}, 3)
                }
            };

            var expectedTotalPrice = cartViewModel.Items.Sum(i => i.Quantity * i.Product.Price);
            var actualTotalPrice = cartViewModel.TotalPrice;
            Assert.Equal(expectedTotalPrice, actualTotalPrice);
        }
    }
}
