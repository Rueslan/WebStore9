using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Infrastructure.Mapping;
using WebStore9.Services.Interfaces;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.ViewModels;

namespace WebStore9.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData ProductData) => _productData = ProductData;

        public async Task<IActionResult> Index()
        {
            var products = await _productData.GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return View(new ProductViewModel());

            var product = await _productData.GetProductByIdAsync(id.Value);

            return product is null 
                ? NotFound() 
                : View(product.ToView());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = model.ToProduct();

            product.Brand = await ApplyBrandAsync(model.BrandName);
            product.BrandId = product.Brand?.Id;

            product.Section = await ApplySectionAsync(model.SectionName);
            product.SectionId = product.Section.Id;

            if (model.Id == 0)
                await _productData.AddProductAsync(product);
            else
                await _productData.UpdateAsync(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var product = await _productData.GetProductByIdAsync(id);
            if (product is null)
                return BadRequest();

            await _productData.DeleteProductByIdAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<Section> ApplySectionAsync(string modelSectionName)
        {
            var currentSection = await _productData.GetSectionByNameAsync(modelSectionName);

            return currentSection ?? await CreateNewSectionAsync(modelSectionName);
        }

        private async Task<Section> CreateNewSectionAsync(string modelSectionName)
        {
            var sections = await _productData.GetSectionsAsync();
            var sectionMaxOrder = sections.Any() ? sections.Max(p => p.Order) + 1 : 1;

            var newSection = new Section { Name = modelSectionName, Order = ++sectionMaxOrder }; //TODO Section Parents

            await _productData.AddSectionAsync(newSection);
            return newSection;
        }

        private async Task<Brand> ApplyBrandAsync(string brandName)
        {
            var currentBrand = await _productData.GetBrandByNameAsync(brandName);

            return currentBrand ?? await CreateNewBrandAsync(brandName);
        }

        private async Task<Brand> CreateNewBrandAsync(string brandName)
        {
            var brands = await _productData.GetBrandsAsync();
            var brandMaxOrder = brands.Any() ? brands.Max(p => p.Order) + 1 : 1;
                
            var newBrand = new Brand { Name = brandName, Order = ++brandMaxOrder };

            await _productData.AddBrandAsync(newBrand);
            return newBrand;
        }
    }
}
