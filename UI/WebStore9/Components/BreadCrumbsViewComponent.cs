using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain.ViewModels;

namespace WebStore9.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _productData;

        public BreadCrumbsViewComponent(IProductData productData) => _productData = productData;

        public IViewComponentResult Invoke()
        {
            var model = new BreadCrumbsViewModel();

            if (int.TryParse(Request.Query["SectionId"], out var sectionId))
            {
                model.Section = _productData.GetSectionById(sectionId);
                if (model.Section.ParentId is {} parentSectionId) 
                    model.Section.Parent = _productData.GetSectionById(parentSectionId);
            }

            if (int.TryParse(Request.Query["BrandId"], out var brandId)) 
                model.Brand = _productData.GetBrandById(brandId);

            if (int.TryParse(ViewContext.RouteData.Values["id"]?.ToString(), out var productId)) 
                model.ProductName = _productData.GetProductById(productId)?.Name;

            return View(model);
        }
    }
}
