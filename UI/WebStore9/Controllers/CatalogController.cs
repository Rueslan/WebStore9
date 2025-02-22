using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;
using WebStore9Domain;
using WebStore9.Services.Mapping;

namespace WebStore9.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData) => _ProductData = ProductData;

        public async Task<IActionResult> Index(int? BrandId, int? SectionId)
        {
            var Filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
            };

            var products = await _ProductData.GetProductsAsync(Filter);

            var view_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.OrderBy(p => p.Order).ToView()  
            };

            return View(view_model);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var product = await _ProductData.GetProductByIdAsync(Id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }

    }
}
