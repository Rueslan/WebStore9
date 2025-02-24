using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces.Services;
using WebStore9Domain;

namespace WebStore9.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData) => _productData = productData;

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var sections = _productData.GetSections();
            return Ok(sections);
        }

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSectionById(int id)
        {
            var section = _productData.GetSectionById(id);
            return Ok(section);
        }

        [HttpGet("sections/{name}")]
        public IActionResult GetSectionByName(string name)
        {
            var section = _productData.GetSectionByName(name);

            return section is null ? NotFound() : Ok(section);
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _productData.GetBrands();
            return Ok(brands);
        }

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrandById(int id)
        {
            var brand = _productData.GetBrandById(id);

            return brand is null ? NotFound() : Ok(brand);
        }

        [HttpGet("brands/{name}")]
        public IActionResult GetBrandByName(string name)
        {
            var brand = _productData.GetBrandByName(name);

            return brand is null ? NotFound() : Ok(brand);
        }

        [HttpPost] //cuz complicated object
        public IActionResult GetProducts([FromBody] ProductFilter filter = null)
        {
            var products = _productData.GetProducts(filter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productData.GetProductById(id);
            return product is null ? NotFound() : Ok(product);
        }

    }
}
