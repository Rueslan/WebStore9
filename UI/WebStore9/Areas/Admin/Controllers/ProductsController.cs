using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Mapping;
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

        public ProductsController(IProductData productData) => _productData = productData;

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
            var product = _productData.GetProductById(id);
            if (product is null)
                return BadRequest();

            _productData.DeleteProductById(id);

            return RedirectToAction(nameof(Index));
        }

        private Section ApplySection(string modelSectionName)
        {
            var currentSection = _productData.GetSectionByName(modelSectionName);

            return currentSection ?? CreateNewSection(modelSectionName);
        }

        private Section CreateNewSection(string modelSectionName)
        {
            var sections =  _productData.GetSections();
            var sectionMaxOrder = sections.Any() ? sections.Max(p => p.Order) + 1 : 1;

            var newSection = new Section { Name = modelSectionName, Order = ++sectionMaxOrder }; //TODO Section Parents

            _productData.AddSection(newSection);
            return newSection;
        }

        private Brand ApplyBrand(string brandName)
        {
            var currentBrand = _productData.GetBrandByName(brandName);

            return currentBrand ?? CreateNewBrand(brandName);
        }

        private Brand CreateNewBrand(string brandName)
        {
            var brands = _productData.GetBrands();
            var brandMaxOrder = brands.Any() ? brands.Max(p => p.Order) + 1 : 1;
                
            var newBrand = new Brand { Name = brandName, Order = ++brandMaxOrder };

            _productData.AddBrand(newBrand);
            return newBrand;
        }
    }
}
