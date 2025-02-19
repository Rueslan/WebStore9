using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Infrastructure.Mapping;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData ProductData) => _productData = ProductData;

        public IActionResult Index()
        {
            var products = _productData.GetProducts();
            return View(products);
        }

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new ProductViewModel());

            var product = _productData.GetProductById(id.Value);

            return product is null 
                ? NotFound() 
                : View(product.ToView());
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = model.ToProduct();

            product.Brand = ApplyBrand(model.BrandName);
            product.BrandId = product.Brand?.Id;

            product.Section = ApplySection(model.SectionName);
            product.SectionId = product.Section.Id;

            if (model.Id == 0)
                _productData.AddProduct(product);
            else
                _productData.Update(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0 || _productData.GetProductById(id) is null)
                return BadRequest();

            _productData.DeleteProductById(id);

            return RedirectToAction(nameof(Index));
        }

        private Section ApplySection(string modelSectionName)
        {
            var currentSection = _productData.GetSectionByName(modelSectionName);

            if (_productData.GetSections().Contains(currentSection))
                return currentSection;
            else
                return CreateNewSection(modelSectionName);
        }

        private Section CreateNewSection(string modelSectionName)
        {
            var sectionMaxOrder = _productData.GetSections().Any()
                ? _productData.GetSections().Max(p => p.Order) + 1
                : 1;

            var newSection = new Section { Name = modelSectionName, Order = ++sectionMaxOrder }; //TODO Section Parents

            _productData.AddSection(newSection);
            return newSection;
        }

        private Brand ApplyBrand(string brandName)
        {
            var currentBrand = _productData.GetBrandByName(brandName);

            if (_productData.GetBrands().Contains(currentBrand))
                return currentBrand;
            else
                return CreateNewBrand(brandName);
        }

        private Brand CreateNewBrand(string brandName)
        {
            var brandMaxOrder = _productData.GetBrands().Any()
                ? _productData.GetBrands().Max(p => p.Order) + 1
                : 1; 
                
            var newBrand = new Brand { Name = brandName, Order = ++brandMaxOrder };

            _productData.AddBrand(newBrand);
            return newBrand;
        }
    }
}
