using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter Filter = null);

        Product GetProductById(int Id);

        Brand GetBrandById(int Id);

        Brand GetBrandByName(string name);

        Section GetSectionById(int Id);

        Section GetSectionByName(string modelSectionName);

        bool DeleteProductById(int Id);

        int AddProduct(Product product);

        void Update(Product product);

        int AddBrand(Brand brand);

        int AddSection(Section section);
    }
}
