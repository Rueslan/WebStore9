using WebStore9Domain.Entities;

namespace WebStore9Domain.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public BrandDTO Brand { get; set; }

        public SectionDTO Section { get; set; }
    }

    public class BrandDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }

    public class SectionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int? ParentId { get; set; }
    }

    public static class BrandDTOMapper
    {
        public static BrandDTO ToDTO(this Brand brand) => brand is null
            ? null
            : new()
            {
                Id = brand.Id,
                Name = brand.Name,
                Order = brand.Order
            };

        public static Brand FromDTO(this BrandDTO brandDto) => brandDto is null
        ? null
        : new ()
        {
            Id = brandDto.Id,
            Name = brandDto.Name,
            Order = brandDto.Order
        };

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> brands) => brands.Select(ToDTO);

        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> brandsDTO) => brandsDTO.Select(FromDTO);
    }

    public static class SectionDTOMapper
    {
        public static SectionDTO ToDTO(this Section section) => section is null
            ? null
            : new()
            {
                Id = section.Id,
                Name = section.Name,
                Order = section.Order,
                ParentId = section.ParentId,
            };

        public static Section FromDTO(this SectionDTO sectionDto) => sectionDto is null
            ? null
            : new()
            {
                Id = sectionDto.Id,
                Name = sectionDto.Name,
                Order = sectionDto.Order,
                ParentId = sectionDto.ParentId,
            };

        public static IEnumerable<SectionDTO> ToDTO(this IEnumerable<Section> sections) => sections.Select(ToDTO);

        public static IEnumerable<Section> FromDTO(this IEnumerable<SectionDTO> sectionsDTO) => sectionsDTO.Select(FromDTO);
    }

    public static class ProductDTOMapper
    {
        public static ProductDTO ToDTO(this Product product) => product is null
            ? null
            : new()
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                Brand = product.Brand.ToDTO(),
                Section = product.Section.ToDTO(),
                ImageUrl = product.ImageUrl,
            };

        public static Product FromDTO(this ProductDTO productDto) => productDto is null
            ? null
            : new()
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Order = productDto.Order,
                Price = productDto.Price,
                BrandId = productDto.Brand?.Id,
                Brand = productDto.Brand.FromDTO(),
                SectionId = productDto.Section.Id,
                Section = productDto.Section.FromDTO(),
                ImageUrl = productDto.ImageUrl,
            };

        public static IEnumerable<Product> FromDTO(this IEnumerable<ProductDTO> productsDTO) => productsDTO.Select(FromDTO);

        public static IEnumerable<ProductDTO> ToDTO(this IEnumerable<Product> products) => products.Select(ToDTO);

    }

}
