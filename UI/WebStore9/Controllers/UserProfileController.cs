using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;

namespace WebStore9.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService orderService)
        {
            var orders = await orderService.GetUserOrders(User.Identity.Name);

            var result = orders.Select(o => new UserOrderViewModel
            {
                Id = o.Id,
                Phone = o.Phone,
                Address = o.Address,
                Description = o.Description,
                TotalPrice = o.TotalPrice,
                Date = o.Date,
            });

            return View(result);
        }
    }
}