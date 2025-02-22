using WebStore9.Interfaces.Services;
using WebStore9.Services.Data;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        public Task<IEnumerable<Brand>> GetBrandsAsync() =>
            Task.FromResult(TestData.Brands);

        public Task<Brand> GetBrandByIdAsync(int Id) =>
            Task.FromResult(TestData.Brands.FirstOrDefault(b => b.Id == Id));

        public Task<Brand> GetBrandByNameAsync(string name) =>
            Task.FromResult(TestData.Brands.FirstOrDefault(b => b.Name == name));

        public Task<IEnumerable<Product>> GetProductsAsync(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);

            return Task.FromResult(query);
        }

        public Task<Product> GetProductByIdAsync(int Id) =>
            Task.FromResult(TestData.Products.FirstOrDefault(p => p.Id == Id));

        public Task<IEnumerable<Section>> GetSectionsAsync() =>
            Task.FromResult(TestData.Sections);

        public Task<Section> GetSectionByIdAsync(int Id) =>
            Task.FromResult(TestData.Sections.FirstOrDefault(s => s.Id == Id));

        public Task<Section> GetSectionByNameAsync(string modelSectionName) =>
            Task.FromResult(TestData.Sections.FirstOrDefault(s => s.Name == modelSectionName));

        public async Task DeleteProductByIdAsync(int Id)
        {
            var product = await GetProductByIdAsync(Id);

            if (product is not null)
                TestData.Products = TestData.Products.Where(p => p != product);
        }

        public Task<int> AddProductAsync(Product product)
        {
            TestData.Products = TestData.Products.Concat([product]);

            return Task.FromResult(product.Id);
        }

        public Task UpdateAsync(Product product)
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

            return Task.CompletedTask;
        }

        public Task<int> AddBrandAsync(Brand brand)
        {
            TestData.Brands = TestData.Brands.Concat([brand]);

            return Task.FromResult(brand.Id);
        }

        public Task<int> AddSectionAsync(Section section)
        {
            TestData.Sections = TestData.Sections.Concat([section]);

            return Task.FromResult(section.Id);
        }
    }
}
