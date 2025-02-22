using Microsoft.AspNetCore.Mvc;

namespace WebStore9.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult WebStoreBlog() => View();
    }
}
