using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebStore9.Controllers;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class UserProfileControllerTests
    {
        [TestMethod]
        public async Task OrdersReturnsCorrectViewModel()
        {
            var controller = new UserProfileController();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, "Test User")
                    }))
                }
            };

            var orderServiceMock = new Mock<IOrderService>();

            var testSequence = new Order[2];

            testSequence[0] = new Order
            {
                Id = 0,
                User = new User(),
                Phone = "Test Phone",
                Address = "Test Address",
                Description = "Test Description",
                Date = DateTimeOffset.MinValue,
                Items = new List<OrderItem>()
                {
                    new OrderItem { Id = 0, Product = new Product(), Price = 1000, Quantity = 1, },
                    new OrderItem { Id = 1, Product = new Product(), Price = 3500, Quantity = 2, },
                }
            };

            testSequence[1] = new Order
            {
                Id = 1,
                User = new User(),
                Phone = "Test Phone2",
                Address = "Test Address2",
                Description = "Test Description2",
                Date = DateTimeOffset.MaxValue,
                Items = new List<OrderItem>()
                {
                    new OrderItem { Id = 2, Product = new Product(), Price = 500, Quantity = 3, },
                    new OrderItem { Id = 3, Product = new Product(), Price = 800, Quantity = 2, },
                }
            };
            
            orderServiceMock.Setup(s => s.GetUserOrders("Test User"))
                .ReturnsAsync(testSequence);

            var expectedUserOrderViewModel = new UserOrderViewModel
            {
                Id = 1,
                Phone = "Test Phone2",
                Address = "Test Address2",
                TotalPrice = 3100,
                Description = "Test Description2",
                Date = DateTimeOffset.MaxValue,
            };
            
            var result = await controller.Orders(orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<UserOrderViewModel>>(viewResult.Model);
            var actualUserOrderViewModel = model.ElementAt(1);

            Assert.Equal(expectedUserOrderViewModel.Id, actualUserOrderViewModel.Id);
            Assert.Equal(expectedUserOrderViewModel.Phone, actualUserOrderViewModel.Phone);
            Assert.Equal(expectedUserOrderViewModel.Address, actualUserOrderViewModel.Address);
            Assert.Equal(expectedUserOrderViewModel.TotalPrice, actualUserOrderViewModel.TotalPrice);
            Assert.Equal(expectedUserOrderViewModel.Description, actualUserOrderViewModel.Description);
            Assert.Equal(expectedUserOrderViewModel.Date, actualUserOrderViewModel.Date);

            Assert.Equal(2, model.Count());

            orderServiceMock.Verify(s => s.GetUserOrders("Test User"));
            orderServiceMock.VerifyNoOtherCalls();
        }
    }
}
