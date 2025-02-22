using Microsoft.AspNetCore.Mvc;
using WebStore9.Services.Interfaces;
using WebStore9Domain.ViewModels;

namespace WebStore9.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BrandsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public async Task<IViewComponentResult> InvokeAsync() => View(await GetBrandsAsync());

        private async Task<IEnumerable<BrandViewModel>> GetBrandsAsync()
        {
            var brands = await _ProductData.GetBrandsAsync();

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
