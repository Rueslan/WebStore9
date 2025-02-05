using Microsoft.AspNetCore.Mvc;
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
                Products = products
                    .OrderBy(p => p.Order)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                    })
            };

            return View(view_model);
        }
    }
}
