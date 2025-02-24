using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.TestAPI;

namespace WebStore9.Controllers
{
    public class WebAPIController : Controller
    {
        private readonly IValuesService _valuesService;

        public WebAPIController(IValuesService valuesService)
        {
            _valuesService = valuesService;
        }
        public IActionResult Index()
        {
            var values = _valuesService.GetAll();
            return View(values);
        }
    }
}
