using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebStore9.Controllers;
using WebStore9.Interfaces.Services;
using WebStore9Domain.Entities.Orders;
using WebStore9Domain.ViewModels;
using Assert = Xunit.Assert;

namespace WebStore9.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CheckOutModelStateInvalidReturnsViewWithModel()
        {
            const string expectedDescription = "Test description";

            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartServiceMock.Object);
            controller.ModelState.AddModelError("error","Invalid model");

            var orderModel = new OrderViewModel
            {
                Description = expectedDescription,
            };

            var result = controller.CheckOut(orderModel, orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CartOrderViewModel>(viewResult.Model);

            Assert.Equal(expectedDescription, model.Order.Description);

            cartServiceMock.Verify(s => s.GetViewModel());
            orderServiceMock.VerifyNoOtherCalls();
            cartServiceMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void CheckOutModelStateValidCallServiceAndReturnsRedirect()
        {
            const string expectedUser = "Test user";
            const string expectedDescription = "Test description";
            const string expectedAddress = "Test address";
            const string expectedPhone = "Test phone";
            const int expectedOrderId = 1;

            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(s => s.GetViewModel())
                .Returns(new CartViewModel
                {
                    Items = new[] {(new ProductViewModel{Name = "Test product"}, 1)}
                });

            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(s => s.CreateOrder(It.IsAny<string>(), It.IsAny<CartViewModel>(), It.IsAny<OrderViewModel>()))
                .ReturnsAsync(new Order
                {
                    Id = expectedOrderId,
                    Phone = expectedPhone,
                    Address = expectedAddress,
                    Description = expectedDescription,
                    Date = DateTime.Now,
                    Items = Array.Empty<OrderItem>()
                });

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new[]{new Claim(ClaimTypes.Name, expectedUser)}))
                    }
                }
            };

            var orderModel = new OrderViewModel
            {
                Address = expectedAddress,
                Phone = expectedPhone,
                Description = expectedDescription,

            };

            var result = controller.CheckOut(orderModel, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}
