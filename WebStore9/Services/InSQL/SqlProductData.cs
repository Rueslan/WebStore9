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
            IQueryable<Product> query = _db.Products;

            if (Filter?.SectionId != null)
                query = query.Where(p => p.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(p => p.BrandId == Filter.BrandId);

            return query;
        }
    }
}
