using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;

namespace WebStore9.Data
{
    public class WebStore9DBInitializer
    {
        private readonly WebStore9DB _db;
        private readonly ILogger<WebStore9DBInitializer> _logger;

        public WebStore9DBInitializer(WebStore9DB db, ILogger<WebStore9DBInitializer> Logger)
        {
            _db = db;
            _logger = Logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Запуск инициализации БД"); 
            //var deleted = await _db.Database.EnsureDeletedAsync();
            //var db_created = await _db.Database.EnsureCreatedAsync();

            var pending_migrations = await _db.Database.GetPendingMigrationsAsync();
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

            if (pending_migrations.Any())
            {

                _logger.LogInformation("Применение миграций: {0}", string.Join(",", pending_migrations));
                await _db.Database.MigrateAsync();
            }

            await InitializeProductsAsync();
        }

        public async Task InitializeProductsAsync()
        {
            var timer = Stopwatch.StartNew();

            if (_db.Sections.Any())
            {
                _logger.LogInformation("Инициализация БД информацией о товарах не требуется");
                return;
            }

            var sections_pool = TestData.Sections.ToDictionary(s => s.Id);
            var brands_pool = TestData.Brands.ToDictionary(s => s.Id);

            foreach (var child_section in TestData.Sections.Where(s => s.ParentId is not null))
            {
                child_section.Parent = sections_pool[(int)child_section.ParentId!];
            }

            foreach (var product in TestData.Products)
            {
                product.Section = sections_pool[product.SectionId];
                if (product.BrandId is {} brand_id)
                {
                    product.Brand = brands_pool[brand_id];
                }

                product.Id = 0;
                product.SectionId = 0;
                product.BrandId = null;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            foreach (var brand in TestData.Brands)
            {
                brand.Id = 0;
            }

            foreach (var employee in TestData.Employees)
            {
                employee.Id = 0;
            }


            _logger.LogInformation("Запись данных...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);
                _db.Employees.AddRange(TestData.Employees);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись данных выполнена успешно за {0} мс", timer.Elapsed.TotalMilliseconds);

        }

    }
}
