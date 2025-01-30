using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

    }
}
