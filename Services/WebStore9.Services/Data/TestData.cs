﻿using WebStore9Domain.Entities;

namespace WebStore9.Services.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; set; } = new()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 29, Seniority = 1 },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 35, Seniority = 4 },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович", Age = 44, Seniority = 6 }
        };

        public static IEnumerable<Section> Sections { get; set; } = new[]
        {
            new Section {Id = 1, Name="Спорт", Order = 0},
            new Section {Id = 2, Name="Nike", Order = 0, ParentId = 1},
            new Section {Id = 3, Name="Under Armor", Order = 1, ParentId = 1},
            new Section {Id = 4, Name="Adidas", Order = 2, ParentId = 1},
            new Section {Id = 5, Name="Puma", Order = 3, ParentId = 1},
            new Section {Id = 6, Name="Asics", Order = 4, ParentId = 1},
            new Section {Id = 7, Name="Для Мужчин", Order = 1},
            new Section {Id = 8, Name="Section Fendi", Order = 0, ParentId = 7},
            new Section {Id = 9, Name="Section Guess", Order = 1, ParentId = 7},
            new Section {Id = 10, Name="Valentino", Order = 2, ParentId = 7},
            new Section {Id = 11, Name="Dior", Order = 3, ParentId = 7},
            new Section {Id = 12, Name="Versache", Order = 4, ParentId = 7},
            new Section {Id = 13, Name="Armani", Order = 5, ParentId = 7},
            new Section {Id = 14, Name="Prada", Order = 6, ParentId = 7},
            new Section {Id = 15, Name="D&G", Order = 7, ParentId = 7},
            new Section {Id = 16, Name="Chanel", Order = 8, ParentId = 7},
            new Section {Id = 17, Name="Gucci", Order = 9, ParentId = 7},
            new Section {Id = 18, Name="Для Женщин", Order = 2},
            new Section {Id = 19, Name="Fendi", Order = 0, ParentId = 18},
            new Section {Id = 20, Name="Guess", Order = 1, ParentId = 18},
            new Section {Id = 21, Name="Section Valentino", Order = 2, ParentId = 18},
            new Section {Id = 22, Name="Section Dior", Order = 3, ParentId = 18},
            new Section {Id = 23, Name="Section Versache", Order = 4, ParentId = 18},
            new Section {Id = 24, Name="Для Детей", Order = 3},
            new Section {Id = 25, Name="Мода", Order = 4},
            new Section {Id = 26, Name="Для дома", Order = 5},
            new Section {Id = 27, Name="Интерьер", Order = 6},
            new Section {Id = 28, Name="Одежда", Order = 7},
            new Section {Id = 29, Name="Сумки", Order = 8},
            new Section {Id = 30, Name="Обувь", Order = 9},
        };

        public static IEnumerable<Brand> Brands { get; set; } = new[]
        {
            new Brand{ Id = 1, Name = "Acne", Order = 0 },
            new Brand{ Id = 2, Name = "Grune Erde", Order = 1 },
            new Brand{ Id = 3, Name = "Albiro", Order = 2 },
            new Brand{ Id = 4, Name = "Ronhill", Order = 3 },
            new Brand{ Id = 5, Name = "Oddmolly", Order = 4 },
            new Brand{ Id = 6, Name = "Boudestijn", Order = 5 },
            new Brand{ Id = 7, Name = "Rosch Creative Culture", Order = 6 },
        };

        public static IEnumerable<Product> Products { get; set; } = new[]
        {
            new Product{ Id = 1, Name = "Белое платье", Price = 1025, ImageUrl="product12.jpg", Order = 0, SectionId = 1, BrandId = 1 },
            new Product{ Id = 2, Name = "Розовое платье", Price = 1025, ImageUrl="product11.jpg", Order = 1, SectionId = 1, BrandId = 2 },
            new Product{ Id = 3, Name = "Красное платье", Price = 1025, ImageUrl="product7.jpg", Order = 2, SectionId = 3, BrandId = 3 },
            new Product{ Id = 4, Name = "Джинсы", Price = 1025, ImageUrl="product8.jpg", Order = 3, SectionId = 1, BrandId = 4 },
            new Product{ Id = 5, Name = "Легкая майка", Price = 1025, ImageUrl="product9.jpg", Order = 4, SectionId = 1, BrandId = 3 },
            new Product{ Id = 6, Name = "Легкое голубое поло", Price = 1025, ImageUrl="product6.jpg", Order = 5, SectionId = 1, BrandId = 5 },
            new Product{ Id = 7, Name = "Платье белое", Price = 1025, ImageUrl="product7.jpg", Order = 6, SectionId = 7, BrandId = 6 },
            new Product{ Id = 8, Name = "Костюм кролика", Price = 1025, ImageUrl="product8.jpg", Order = 7, SectionId = 7, BrandId = 7 },
            new Product{ Id = 9, Name = "Красное китайское платье", Price = 1025, ImageUrl="product9.jpg", Order = 8, SectionId = 18, BrandId = 3 },
            new Product{ Id = 10, Name = "Женские джинсы", Price = 1025, ImageUrl="product10.jpg", Order = 9, SectionId = 18, BrandId = 4 },
            new Product{ Id = 11, Name = "Джинсы женские", Price = 1025, ImageUrl="product11.jpg", Order = 10, SectionId = 20, BrandId = 2 },
            new Product{ Id = 12, Name = "Летний костюм", Price = 1025, ImageUrl="product12.jpg", Order = 11, SectionId = 20, BrandId = 6 },
        };
    }
}
