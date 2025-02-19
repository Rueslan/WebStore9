using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Services.Interfaces;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStore9DB _db;

        public SqlProductData(WebStore9DB db) => _db = db;

        public IEnumerable<Section> GetSections() => _db.Sections;

        public IEnumerable<Brand> GetBrands() => _db.Brands;

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
                if (Filter?.SectionId != null)
                    query = query.Where(p => p.SectionId == Filter.SectionId);

                if (Filter?.BrandId != null)
                    query = query.Where(p => p.BrandId == Filter.BrandId);
            }

            return query;
        }

        public Product GetProductById(int Id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == Id);

        public Brand GetBrandById(int Id) => _db.Brands.SingleOrDefault(b => b.Id == Id);

        public Brand GetBrandByName(string name) => _db.Brands.FirstOrDefault(b => b.Name == name);

        public Section GetSectionById(int Id) => _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefault(s => s.Id == Id);

        public Section GetSectionByName(string modelSectionName) => _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefault(s => s.Name == modelSectionName);

        public void DeleteProductById(int Id)
        {
            _db.Products.Remove(GetProductById(Id));
            _db.SaveChanges();
        }

        public int AddProduct(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException(nameof(product));

            if (_db.Products.Contains(product)) return product.Id;

            _db.Products.Add(product);
            _db.SaveChanges();

            return product.Id;
        }

        public void Update(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException(nameof(product));

            var db_product = GetProductById(product.Id);
            if (db_product == null || db_product == product) return;

            db_product.Name = product.Name;
            db_product.Price = product.Price;
            db_product.ImageUrl = product.ImageUrl;
            db_product.Brand = product.Brand;
            db_product.Section = product.Section;
            db_product.BrandId = product.BrandId;
            db_product.SectionId = product.SectionId;

            _db.SaveChanges();
        }

        public int AddBrand(Brand brand)
        {
            if (brand.Id != 0) 
                return brand.Id;

            _db.Brands.Add(brand);
            _db.SaveChanges();

            return brand.Id;
        }

        public int AddSection(Section section)
        {
            if (section == null) 
                throw new ArgumentNullException(nameof(section));

            if (section.Id != 0) 
                return section.Id;

            _db.Sections.Add(section);
            _db.SaveChanges();

            return section.Id;
        }
    }
}
