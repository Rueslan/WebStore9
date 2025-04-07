using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;

namespace WebStore9.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BrandsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke(string brandId)
        {
            ViewBag.BrandId = int.TryParse(brandId, out var id) ? id : (int?)null;
            return View(GetBrandsAsync());
        }

        private IEnumerable<BrandViewModel> GetBrandsAsync()
        {
            var brands = _productData.GetBrands();

            return brands
                .OrderBy(b => b.Order)
                .Select(b => new BrandViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                });
        }
    }
}
