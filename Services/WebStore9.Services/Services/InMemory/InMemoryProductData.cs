using WebStore9.Interfaces.Services;
using WebStore9.Services.Data;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() =>
            TestData.Brands;

        public Brand GetBrandById(int Id) =>
            TestData.Brands.FirstOrDefault(b => b.Id == Id);

        public Brand GetBrandByName(string name) =>
            TestData.Brands.FirstOrDefault(b => b.Name == name);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);

            return query;
        }

        public Product GetProductById(int Id) =>
            TestData.Products.FirstOrDefault(p => p.Id == Id);

        public IEnumerable<Section> GetSections() =>
            TestData.Sections;

        public Section GetSectionById(int Id) =>
            TestData.Sections.FirstOrDefault(s => s.Id == Id);

        public Section GetSectionByName(string modelSectionName) =>
            TestData.Sections.FirstOrDefault(s => s.Name == modelSectionName);

        public bool DeleteProductById(int Id)
        {
            var product = GetProductById(Id);

            if (product is not null)
                TestData.Products = TestData.Products.Where(p => p != product);
            else
                return false;

            return true;
        }

        public int AddProduct(Product product)
        {
            TestData.Products = TestData.Products.Concat([product]);

            return product.Id;
        }

        public void Update(Product product)
        {
            var result = TestData.Products.FirstOrDefault(p => p.Id == product.Id);

            if (result is not null)
            {
                result.Name = product.Name;
                result.Brand = product.Brand;
                result.Price = product.Price;
                result.Section = product.Section;
                result.ImageUrl = product.ImageUrl;
                result.Order = product.Order;
                result.BrandId = product.BrandId;
                result.SectionId = product.SectionId;
            }
        }

        public int AddBrand(Brand brand)
        {
            TestData.Brands = TestData.Brands.Concat([brand]);

            return brand.Id;
        }

        public int AddSection(Section section)
        {
            TestData.Sections = TestData.Sections.Concat([section]);

            return section.Id;
        }
    }
}
