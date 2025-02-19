using WebStore9.Data;
using WebStore9.Services.Interfaces;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public Brand GetBrandById(int Id) => TestData.Brands.FirstOrDefault(b => b.Id == Id);

        public Brand GetBrandByName(string name) => TestData.Brands.FirstOrDefault(b => b.Name == name);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);

            return query;
        }

        public Product GetProductById(int Id) => TestData.Products.FirstOrDefault(p => p.Id == Id);

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public Section GetSectionById(int Id) => TestData.Sections.FirstOrDefault(s => s.Id == Id);

        public Section GetSectionByName(string modelSectionName) =>
            TestData.Sections.FirstOrDefault(s => s.Name == modelSectionName);

        public void DeleteProductById(int Id) => TestData.Products = TestData.Products.Where(p => p != GetProductById(Id));

        public int AddProduct(Product product)
        {
            TestData.Products.Append(product);
            return product.Id;
        }

        public void Update(Product product)
        {
            var result = TestData.Products.FirstOrDefault(p => p.Id == product.Id);
            result = product;
        }

        public int AddBrand(Brand brand)
        {
            TestData.Brands.Append(brand);
            return brand.Id;
        }

        public int AddSection(Section section)
        {
            TestData.Sections.Append(section);
            return section.Id;
        }
    }
}
