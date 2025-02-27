using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Interfaces.Services;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStore9DB _db;

        public SqlProductData(WebStore9DB db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections.ToArray();

        public IEnumerable<Brand> GetBrands() => _db.Brands.ToArray();

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (Filter?.Ids?.Length > 0)
            {
                query = query.Where(p => Filter.Ids.Contains(p.Id));
            }
            else
            {
                if (Filter?.SectionId is not null)
                    query = query.Where(p => p.SectionId == Filter.SectionId);

                if (Filter?.BrandId is not null)
                    query = query.Where(p => p.BrandId == Filter.BrandId);
            }

            return query.ToArray();
        }

        public Product GetProductById(int Id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == Id);

        public Brand GetBrandById(int Id) => _db.Brands.FirstOrDefault(b => b.Id == Id);

        public Brand GetBrandByName(string name) => _db.Brands.FirstOrDefault(b => b.Name == name);

        public Section GetSectionById(int Id) => _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefault(s => s.Id == Id);

        public Section GetSectionByName(string modelSectionName) => _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefault(s => s.Name == modelSectionName);

        public bool DeleteProductById(int Id)
        {
            var product = GetProductById(Id);

            if (product is null)
                return false;

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

            _db.Entry(db_product).CurrentValues.SetValues(product);

            _db.SaveChangesAsync();
        }

        public int AddBrand(Brand brand)
        {
            if (brand.Id != 0 && _db.Brands.Any(b => b.Id == brand.Id))
                return brand.Id;

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

            _db.Sections.AddAsync(section);
            _db.SaveChangesAsync();

            return section.Id;
        }
    }
}
