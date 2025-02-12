using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Services.Interfaces
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter Filter = null);

        Product GetProductById(int Id);

        Brand GetBrandById(int Id);

        Section GetSectionById(int Id);
    }
}
