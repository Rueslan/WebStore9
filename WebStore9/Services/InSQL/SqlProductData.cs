using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;
using WebStore9.Data;
using WebStore9.Services.Interfaces;
using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        private readonly WebStore9DB _db;

        public SqlProductData(WebStore9DB db)
        {
            _db = db;
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

            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);

            return query;
        }

        public Product GetProductById(int Id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == Id);

        public Brand GetBrandById(int Id) => _db.Brands.SingleOrDefault(b => b.Id == Id);

        public Section GetSectionById(int Id) => _db.Sections.SingleOrDefault(s => s.Id == Id);
    }
}
