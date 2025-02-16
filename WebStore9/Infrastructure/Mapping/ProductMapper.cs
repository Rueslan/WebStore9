using WebStore9.ViewModels;
using WebStore9Domain.Entities;

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
                BrandId = product.BrandId,
                SectionName = product.Section.Name,
                SectionId = product.SectionId,
                Order = product.Order,
            };

        public static IEnumerable<ProductViewModel> ToView(this IEnumerable<Product> products) => products.Select(ToView);

        public static Product ToProduct(this ProductViewModel product_viewModel) => product_viewModel == null
            ? null
            : new Product
            {
                Id = product_viewModel.Id,
                Name = product_viewModel.Name,
                ImageUrl = product_viewModel.ImageUrl,
                Price = product_viewModel.Price,
                BrandId = product_viewModel.BrandId,
                SectionId = product_viewModel.SectionId,
                Order = product_viewModel.Order,
            };

    }
}
