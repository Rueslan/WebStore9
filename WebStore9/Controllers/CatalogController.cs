using Microsoft.AspNetCore.Mvc;
using WebStore9.Infrastructure.Mapping;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain;

namespace WebStore9.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IActionResult Index(int? BrandId, int? SectionId)
        {
            var Filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
            };

            var products = _ProductData.GetProducts(Filter);

            var view_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView()  
            };

            return View(view_model);
        }

        public IActionResult Details(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }

    }
}
