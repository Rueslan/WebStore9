using Microsoft.AspNetCore.Mvc;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services;
using WebStore9Domain;
using WebStore9Domain.DTO;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Controllers
{
    [Route(WebAPIAddresses.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IProductData _productData;

        public ProductsApiController(IProductData productData) => _productData = productData;

        [HttpGet("sections")]
        public IActionResult GetSections()
        {
            var sections = _productData.GetSections();
            return Ok(sections.ToDTO());
        }

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSectionById(int id)
        {
            var section = _productData.GetSectionById(id);
            return Ok(section.ToDTO());
        }

        [HttpGet("sections/{name}")]
        public IActionResult GetSectionByName(string name)
        {
            var section = _productData.GetSectionByName(name);

            return section is null ? NotFound() : Ok(section.ToDTO());
        }

        [HttpGet("brands")]
        public IActionResult GetBrands()
        {
            var brands = _productData.GetBrands();
            return Ok(brands.ToDTO());
        }

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrandById(int id)
        {
            var brand = _productData.GetBrandById(id);

            return brand is null ? NotFound() : Ok(brand.ToDTO());
        }

        [HttpGet("brands/{name}")]
        public IActionResult GetBrandByName(string name)
        {
            var brand = _productData.GetBrandByName(name);

            return brand is null ? NotFound() : Ok(brand.ToDTO());
        }

        [HttpPost] //cuz complicated object
        public IActionResult GetProducts([FromBody] ProductFilter filter = null)
        {
            var products = _productData.GetProducts(filter);
            return Ok(products.ToDTO());
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productData.GetProductById(id);
            return product is null ? NotFound() : Ok(product.ToDTO());
        }

        [HttpPost("products/add")]
        public IActionResult AddProduct(Product product)
        {
            var id = _productData.AddProduct(product);
            return CreatedAtAction(nameof(GetProduct), new { id }, product);
        }

        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            _productData.Update(product);
            return Ok(product.ToDTO());
        }

        [HttpPost("brands/add")]
        public IActionResult AddBrand(Brand brand)
        {
            var id = _productData.AddBrand(brand);
            return CreatedAtAction(nameof(GetBrandById), new { id }, brand);
        }

        [HttpPost("sections/add")]
        public IActionResult AddSection(Section section)
        {
            var id = _productData.AddSection(section);
            return CreatedAtAction(nameof(GetSectionById), new { id }, section);
        }

    }
}
