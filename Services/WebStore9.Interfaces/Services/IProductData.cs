using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter filter = null);

        Product GetProductById(int id);

        Brand GetBrandById(int id);

        Brand GetBrandByName(string name);

        Section GetSectionById(int id);

        Section GetSectionByName(string modelSectionName);

        bool DeleteProductById(int id);

        int AddProduct(Product product);

        void Update(Product product);

        int AddBrand(Brand brand);

        int AddSection(Section section);
    }
}
