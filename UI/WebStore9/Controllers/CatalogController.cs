using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;
using WebStore9Domain;
using WebStore9.Services.Mapping;

namespace WebStore9.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _productData;

        public CatalogController(IProductData productData) => _productData = productData;

        public IActionResult Index(int? brandId, int? sectionId)
        {
            var Filter = new ProductFilter
            {
                BrandId = brandId,
                SectionId = sectionId,
            };

            var products = _productData.GetProducts(Filter);

            var viewModel = new CatalogViewModel
            {
                BrandId = brandId,
                SectionId = sectionId,
                Products = products.OrderBy(p => p.Order).ToView()  
            };

            return View(viewModel);
        }

        public IActionResult Details(int Id)
        {
            var product = _productData.GetProductById(Id);

            if (product is null)
                return NotFound();

            return View(product.ToView());
        }

    }
}
