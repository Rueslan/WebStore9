using Microsoft.Extensions.Logging;
using Moq;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Services;
using WebStore9Domain;
using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore9.Services.Tests.Services
{
    [TestClass]
    public class CartServiceTests
    {
        private Cart _cart;

        private Mock<IProductData> _productDataMock;
        private Mock<ICartStore> _cartStoreMock;

        private ICartService _cartService;

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

            _productDataMock = new Mock<IProductData>();
            _productDataMock
                .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(new[]
                {
                    new Product()
                    {
                        Id = 1,
                        Name = "Product 1",
                        Price = 1.1m,
                        Order = 0,
                        ImageUrl = "Product1.png",
                        Brand = new Brand() { Id = 1, Name = "Brand 1" },
                        Section = new Section { Id = 1, Name = "Section 1"}
                    },
                    new Product()
                    {
                        Id = 2,
                        Name = "Product 2",
                        Price = 2.2m,
                        Order = 0,
                        ImageUrl = "Product2.png",
                        Brand = new Brand { Id = 2, Name = "Brand 2" },
                        Section = new Section { Id = 2, Name = "Section 2"}
                    },
                    new Product()
                    {
                        Id = 3,
                        Name = "Product 3",
                        Price = 3.3m,
                        Order = 0,
                        ImageUrl = "Product3.png",
                        Brand = new Brand { Id = 3, Name = "Brand 3" },
                        Section = new Section { Id = 3, Name = "Section 3"}
                    },
                });

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.cart).Returns(_cart);
            var _cartLoggerMock = new Mock<ILogger<CartService>>();

            _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object, _cartLoggerMock.Object);
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

        [TestMethod]
        public void CartServiceAddWorksCorrect()
        {
            _cart.Items.Clear();

            const int expectedId = 5;
            const int expectedItemsCount = 1;

            _cartService.Add(expectedId);

            var actualItemsCount = _cart.ItemsCount;

            Assert.Equal(expectedItemsCount, actualItemsCount);

            Assert.Single(_cart.Items);

            Assert.Equal(expectedId, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartServiceRemovesCorrectItem()
        {
            const int itemId = 1;
            const int expectedProductId = 2;

            _cartService.Remove(itemId);

            Assert.Single(_cart.Items);

            Assert.Equal(expectedProductId, _cart.Items.Single().ProductId);
        }

        [TestMethod]
        public void CartServiceClearCart()
        {
            _cartService.Clear();

            Assert.Empty(_cart.Items);
        }

        [TestMethod]
        public void CarServiceDecrementCorrect()
        {
            const int itemId = 2;
            const int expectedQuantity = 2;
            const int expectedItemsCount = 3;
            const int expectedProductsCount = 2;

            _cartService.Decrement(itemId);

            var items = _cart.Items.ToArray();
            Assert.Equal(itemId, items[1].ProductId);
            Assert.Equal(expectedQuantity, items[1].Quantity);
            Assert.Equal(expectedItemsCount, _cart.ItemsCount);
            Assert.Equal(expectedProductsCount, _cart.Items.Count);
        }

        [TestMethod]
        public void CartServiceRemovesItemWhenDecrementIs0()
        {
            const int itemId = 1;
            const int expectedItemsCount = 3;

            _cartService.Decrement(itemId);

            Assert.Equal(expectedItemsCount, _cart.ItemsCount);
            Assert.Single(_cart.Items);
        }

        [TestMethod]
        public void CartServiceGetsViewModelCorrect()
        {
            const int expectedItemsCount = 4;
            const decimal expectedFirstProductPrice = 1.1m;

            var result = _cartService.GetViewModel();

            Assert.Equal(expectedItemsCount, result.ItemsCount);
            Assert.Equal(expectedFirstProductPrice,result.Items.First().Product.Price);
        }
    }
}
