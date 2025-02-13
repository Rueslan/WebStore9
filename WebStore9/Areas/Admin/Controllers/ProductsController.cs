using Microsoft.AspNetCore.Mvc;
using WebStore9.Services.Interfaces;

namespace WebStore9.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData ProductData)
        {
            _productData = ProductData;
        }

        public IActionResult Index()
        {
            var products = _productData.GetProducts();

            return View(products);
        }
    }
}
