using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Data;
using WebStore9.Infrastructure.Mapping;
using WebStore9.Services.Interfaces;
using WebStore9.ViewModels;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStore9DB _db;
        private int _CurrentMaxId;

        public SqlProductData(WebStore9DB db)
        {
            _db = db;
            _CurrentMaxId = _db.Products.Max(e => e.Id);
        }

        public IEnumerable<Section> GetSections()
        {
            return _db.Sections;
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _db.Brands;
        }

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

        public Section GetSectionById(int Id) => _db.Sections.SingleOrDefault(s => s.Id == Id);

        public void DeleteProductById(int Id)
        {
            _db.Products.Remove(GetProductById(Id));
            _db.SaveChanges();
        }

        public int Add(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            if (_db.Products.Contains(product)) return product.Id;

            product.Id = ++_CurrentMaxId;
            _db.Products.Add(product);

            return product.Id;
        }

        public void Update(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

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
    }
}
