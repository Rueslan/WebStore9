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

        public async Task<IEnumerable<Section>> GetSectionsAsync() => await _db.Sections.ToArrayAsync();

        public async Task<IEnumerable<Brand>> GetBrandsAsync() => await _db.Brands.ToArrayAsync();

        public async Task<IEnumerable<Product>> GetProductsAsync(ProductFilter Filter = null)
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

            return await query.ToArrayAsync();
        }

        public async Task<Product> GetProductByIdAsync(int Id) => await _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefaultAsync(p => p.Id == Id);

        public async Task<Brand> GetBrandByIdAsync(int Id) => await _db.Brands.FirstOrDefaultAsync(b => b.Id == Id);

        public async Task<Brand> GetBrandByNameAsync(string name) => await _db.Brands.FirstOrDefaultAsync(b => b.Name == name);

        public async Task<Section> GetSectionByIdAsync(int Id) => await _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.Id == Id);

        public async Task<Section> GetSectionByNameAsync(string modelSectionName) => await _db.Sections
            .Include(s => s.Parent)
            .FirstOrDefaultAsync(s => s.Name == modelSectionName);

        public async Task DeleteProductByIdAsync(int Id)
        {
            var product = await GetProductByIdAsync(Id);

            if (product is null) 
                return;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<int> AddProductAsync(Product product)
        {
            if (product is null) 
                throw new ArgumentNullException(nameof(product));

            if (product.Id != 0 && await _db.Products.AnyAsync(p => p.Id == product.Id))
                return product.Id;

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            return product.Id;
        }

        public async Task UpdateAsync(Product product)
        {
            if (product is null) 
                throw new ArgumentNullException(nameof(product));

            var db_product = await GetProductByIdAsync(product.Id);
            if (db_product is null) 
                return;

            _db.Entry(db_product).CurrentValues.SetValues(product);

            await _db.SaveChangesAsync();
        }

        public async Task<int> AddBrandAsync(Brand brand)
        {
            if (brand.Id != 0 && await _db.Brands.AnyAsync(b => b.Id == brand.Id))
                return brand.Id;

            await _db.Brands.AddAsync(brand);
            await _db.SaveChangesAsync();

            return brand.Id;
        }

        public async Task<int> AddSectionAsync(Section section)
        {
            if (section == null) 
                throw new ArgumentNullException(nameof(section));

            if (section.Id != 0 && await _db.Sections.AnyAsync(s => s.Id == section.Id))
                    return section.Id;

            await _db.Sections.AddAsync(section);
            await _db.SaveChangesAsync();

            return section.Id;
        }
    }
}
