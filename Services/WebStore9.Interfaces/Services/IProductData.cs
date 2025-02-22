using WebStore9Domain;
using WebStore9Domain.Entities;

namespace WebStore9.Interfaces.Services
{
    public interface IProductData
    {
        Task<IEnumerable<Section>> GetSectionsAsync();

        Task<IEnumerable<Brand>> GetBrandsAsync();

        Task<IEnumerable<Product>> GetProductsAsync(ProductFilter Filter = null);

        Task<Product> GetProductByIdAsync(int Id);

        Task<Brand> GetBrandByIdAsync(int Id);

        Task<Brand> GetBrandByNameAsync(string name);

        Task<Section> GetSectionByIdAsync(int Id);

        Task<Section> GetSectionByNameAsync(string modelSectionName);

        Task DeleteProductByIdAsync(int Id);

        Task<int> AddProductAsync(Product product);

        Task UpdateAsync(Product product);

        Task<int> AddBrandAsync(Brand brand);

        Task<int> AddSectionAsync(Section section);
    }
}
