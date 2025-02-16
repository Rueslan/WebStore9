using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore9.Infrastructure.Mapping;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Administrators)]
    public class ProductsController : Controller
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData ProductData)
        {
            _productData = ProductData;
        }

        public IActionResult Index()
        {
            var products = _productData.GetProducts();
            return View(products);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new ProductViewModel());

            var product = _productData.GetProductById((int)id);

            if (product == null) return NotFound();

            return View(product.ToView());
        }

        [HttpPost]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var product = model.ToProduct();
            product.Brand = _productData.GetBrandById((int)product.BrandId);
            product.Section = _productData.GetSectionById(product.SectionId);

            if (model.Id == 0)
                _productData.Add(product);
            else
                _productData.Update(product);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int Id)
        {
            if (Id < 0) return BadRequest();

            var product = _productData.GetProductById(Id);

            if (product == null)
                return NotFound();

            _productData.DeleteProductById(Id);

            return RedirectToAction(nameof(Index));
        }

    }
}
