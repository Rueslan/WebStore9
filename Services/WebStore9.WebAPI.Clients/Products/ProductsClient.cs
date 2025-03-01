using System.Net.Http.Json;
using WebStore9.Interfaces;
using WebStore9.Interfaces.Services;
using WebStore9.WebAPI.Clients.Base;
using WebStore9Domain;
using WebStore9Domain.DTO;
using WebStore9Domain.Entities;

namespace WebStore9.WebAPI.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(HttpClient client) : base(client, WebAPIAddresses.Products) { }

        public IEnumerable<Section> GetSections()
        {
            var sections = Get<IEnumerable<SectionDTO>>($"{Address}/sections");
            return sections.FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            var brands = Get<IEnumerable<BrandDTO>>($"{Address}/brands");
            return brands.FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            var response = Post(Address, filter ?? new());
            var productDTOs = response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;

            return productDTOs.FromDTO();
        }

        public Product GetProductById(int id)
        {
            var product = Get<ProductDTO>($"{Address}/{id}");
            return product.FromDTO();
        }

        public Brand GetBrandById(int id)
        {
            var brand = Get<BrandDTO>($"{Address}/brands/{id}");
            return brand.FromDTO();
        }

        public Brand GetBrandByName(string name)
        {
            var brand = Get<BrandDTO>($"{Address}/brands/{name}");
            return brand.FromDTO();
        }

        public Section GetSectionById(int id)
        {
            var section = Get<SectionDTO>($"{Address}/sections/{id}");
            return section.FromDTO();
        }

        public Section GetSectionByName(string name)
        {
            var section = Get<SectionDTO>($"{Address}/sections/{name}");
            return section.FromDTO();
        }

        public bool DeleteProductById(int id)
        {
            var response = Delete($"{Address}/{id}");
            var success = response.IsSuccessStatusCode;
            return success;
        }

        public int AddProduct(Product product)
        {
            var response = Post(Address, product);
            if (!response.IsSuccessStatusCode)
                return -1;

            return product.Id;
        }

        public void Update(Product product)
        {
            var response = Put($"{Address}/{product.Id}", product);
        }

        public int AddBrand(Brand brand)
        {
            var response = Post($"{Address}/brands/", brand);
            if (!response.IsSuccessStatusCode)
                return -1;

            return brand.Id;
        }

        public int AddSection(Section section)
        {
            var response = Post($"{Address}/sections/", section);
            if (!response.IsSuccessStatusCode)
                return -1;

            return section.Id;
        }
    }
}
