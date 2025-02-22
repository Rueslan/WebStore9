using WebStore9Domain.Entities;
using WebStore9Domain.ViewModels;

namespace WebStore9.Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product product) => product is null
            ? null
            : new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                BrandName = product.Brand.Name ??= "",
                SectionName = product.Section.Name,
                Order = product.Order,
            };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);

        public static Product ToProduct(this ProductViewModel productViewModel) => productViewModel is null
            ? null
            : new Product
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                ImageUrl = productViewModel.ImageUrl,
                Price = productViewModel.Price,
                BrandId = productViewModel.BrandId,
                SectionId = productViewModel.SectionId,
                Order = productViewModel.Order,
            };
    }
}
