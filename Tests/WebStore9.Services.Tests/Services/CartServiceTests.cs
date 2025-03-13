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
                });

            _cartStoreMock = new Mock<ICartStore>();
            _cartStoreMock.Setup(c => c.cart).Returns(_cart);

            _cartService = new CartService(_cartStoreMock.Object, _productDataMock.Object, null);
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
