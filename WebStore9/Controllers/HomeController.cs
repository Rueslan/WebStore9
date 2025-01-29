using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult SomeAction(int id) => Content($"First Controller {id}");
    }
}
