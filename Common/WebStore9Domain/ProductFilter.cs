﻿namespace WebStore9Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; set; }
        public int? BrandId { get; set; }

        public int[]? Ids { get; set; }
    }
}
