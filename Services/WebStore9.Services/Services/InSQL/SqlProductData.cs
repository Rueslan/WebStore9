using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore9.DAL.Context;
using WebStore9.Interfaces.Services;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStore9DB _db;
        private readonly ILogger<WebStore9DB> _logger;

        public SqlProductData(WebStore9DB db, ILogger<WebStore9DB> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IEnumerable<Section> GetSections()
        {
            _logger.LogInformation("Получение всех секций из БД");
            return _db.Sections.ToArray();
        }

        public IEnumerable<Brand> GetBrands()
        {
            _logger.LogInformation("Получение всех брендов из БД");
            return _db.Brands.ToArray();
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            _logger.LogInformation("Получение товаров из БД");

            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (filter?.Ids?.Length > 0)
            {
                _logger.LogInformation("Применение фильтра для получения товаров из БД по идентификаторам {0}", string.Join(", ", filter.Ids));
                query = query.Where(p => filter.Ids.Contains(p.Id));
            }
            else
            {
                if (filter?.SectionId is not null)
                {
                    _logger.LogInformation("Применение фильтра для получения товаров из БД по секциии id {0}", filter.SectionId);
                    query = query.Where(p => p.SectionId == filter.SectionId);
                }

                if (filter?.BrandId is not null)
                {
                    _logger.LogInformation("Применение фильтра для получения товаров из БД по бренду id {0}", filter.BrandId);
                    query = query.Where(p => p.BrandId == filter.BrandId);
                }
            }

            return query.ToArray();
        }

        public Product GetProductById(int id)
        {
            _logger.LogInformation("Получение товара из БД по id {0}", id);

            return _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section)
                .FirstOrDefault(p => p.Id == id);
        }

        public Brand GetBrandById(int id)
        {
            _logger.LogInformation("Получение бренда из БД по id {0}", id);

            return _db.Brands.FirstOrDefault(b => b.Id == id);
        }

        public Brand GetBrandByName(string name)
        {
            _logger.LogInformation("Получение бренда из БД по имени {0}", name);

            return _db.Brands.FirstOrDefault(b => b.Name == name);
        }

        public Section GetSectionById(int id)
        {
            _logger.LogInformation("Получение секции из БД по id {0}", id);

            return _db.Sections
                .Include(s => s.Parent)
                .FirstOrDefault(s => s.Id == id);
        }

        public Section GetSectionByName(string modelSectionName)
        {
            _logger.LogInformation("Получение секции из БД по имени {0}", modelSectionName);

            return _db.Sections
                .Include(s => s.Parent)
                .FirstOrDefault(s => s.Name == modelSectionName);
        }

        public bool DeleteProductById(int id)
        {
            var product = GetProductById(id);

            if (product is null)
                return false;

            _logger.LogInformation("Удаление товара из БД по id {0} [{1}]", id, product.ToString());

            _db.Products.Remove(product);
            _db.SaveChanges();

            return true;
        }

        public int AddProduct(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (product.Id != 0 && _db.Products.Any(p => p.Id == product.Id))
                return product.Id;

            _logger.LogInformation("Добавление товара в БД {0}", product.ToString());

            _db.Products.AddAsync(product);
            _db.SaveChangesAsync();

            return product.Id;
        }

        public void Update(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            var db_product = GetProductById(product.Id);
            if (db_product is null)
                return;

            _logger.LogInformation("изменение товара в БД {0} \n на новый товар {1}", db_product.ToString(), product.ToString());

            _db.Entry(db_product).CurrentValues.SetValues(product);

            _db.SaveChangesAsync();
        }

        public int AddBrand(Brand brand)
        {
            if (brand.Id != 0 && _db.Brands.Any(b => b.Id == brand.Id))
                return brand.Id;

            _logger.LogInformation("Добавление бренда в БД {0}", brand.Name);

            _db.Brands.AddAsync(brand);
            _db.SaveChangesAsync();

            return brand.Id;
        }

        public int AddSection(Section section)
        {
            if (section == null)
                throw new ArgumentNullException(nameof(section));

            if (section.Id != 0 && _db.Sections.Any(s => s.Id == section.Id))
                return section.Id;

            _logger.LogInformation("Добавление секции в БД {0}", section.Name);

            _db.Sections.AddAsync(section);
            _db.SaveChangesAsync();

            return section.Id;
        }
    }
}
