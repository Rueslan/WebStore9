using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.TestAPI;

namespace WebStore9.Controllers
{
    public class WebAPIController(IValuesService valuesService) : Controller
    {
        public IActionResult Index()
        {
            var values = valuesService.GetAll();
            return View(values);
        }
    }
}
