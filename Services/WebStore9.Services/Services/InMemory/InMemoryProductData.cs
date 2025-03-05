using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Data;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InMemory
{
    public class InMemoryProductData : IProductData
    {
        private readonly ILogger<InMemoryProductData> _logger;

        public InMemoryProductData(ILogger<InMemoryProductData> logger) => _logger = logger;

        public IEnumerable<Brand> GetBrands()
        {
            _logger.LogInformation("Получение всех брендов из памяти");
            return TestData.Brands;
        }

        public Brand GetBrandById(int id)
        {
            _logger.LogInformation("Получение бренда из памяти по id: {0}", id);
            return TestData.Brands.FirstOrDefault(b => b.Id == id);
        }

        public Brand GetBrandByName(string name)
        {
            _logger.LogInformation("Получение бренда из памяти по имени: {0}", name);
            return TestData.Brands.FirstOrDefault(b => b.Name == name);
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            _logger.LogInformation("Получение всех товаров из памяти");
            IEnumerable<Product> query = TestData.Products;

            if (filter?.Ids?.Length > 0)
            {
                _logger.LogInformation("Применение фильтра для получения товаров из памяти по идентификаторам {0}", string.Join(", ", filter.Ids));
                query = query.Where(p => filter.Ids.Contains(p.Id));
            }
            else
            {
                if (filter?.SectionId != null)
                {
                    _logger.LogInformation("Применение фильтра для получения товаров из памияти по секциии id {0}", filter.SectionId);
                    query = query.Where(p => p.SectionId == filter.SectionId);
                }

                if (filter?.BrandId != null)
                {
                    _logger.LogInformation("Применение фильтра для получения товаров из памяти по бренду id {0}", filter.BrandId);
                    query = query.Where(p => p.BrandId == filter.BrandId);
                }
            }

            return query;
        }

        public Product GetProductById(int id)
        {
            _logger.LogInformation("Получение товара из памяти по id {0}", id);
            return TestData.Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Section> GetSections()
        {
            _logger.LogInformation("Получение всех секций из памяти");
            return TestData.Sections;
        }

        public Section GetSectionById(int id)
        {
            _logger.LogInformation("Получение секции из памяти по id {0}", id);
            return TestData.Sections.FirstOrDefault(s => s.Id == id);
        }

        public Section GetSectionByName(string modelSectionName)
        {
            _logger.LogInformation("Получение секции из памяти по имени {0}", modelSectionName);
            return TestData.Sections.FirstOrDefault(s => s.Name == modelSectionName);
        }

        public bool DeleteProductById(int id)
        {
            var product = GetProductById(id);

            if (product is not null)
                TestData.Products = TestData.Products.Where(p => p != product);
            else
                return false;

            _logger.LogInformation("Удаление товара из памяти по id {0}", id);

            return true;
        }

        public int AddProduct(Product product)
        {
            _logger.LogInformation("Добавление товара в память {0}", product.ToString());

            TestData.Products = TestData.Products.Concat([product]);

            return product.Id;
        }

        public void Update(Product product)
        {
            var result = TestData.Products.FirstOrDefault(p => p.Id == product.Id);

            if (result is not null)
            {
                _logger.LogInformation("изменение товара в памяти {0} \n на новый товар {1}", result.ToString(), product.ToString());

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
            _logger.LogInformation("Добавление бренда в память {0}", brand.Name);

            TestData.Brands = TestData.Brands.Concat([brand]);

            return brand.Id;
        }

        public int AddSection(Section section)
        {
            _logger.LogInformation("Добавление секции в память {0}", section.Name);

            TestData.Sections = TestData.Sections.Concat([section]);

            return section.Id;
        }
    }
}
