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

            var pending_migrations = await _db.Database.GetAppliedMigrationsAsync();
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
            _logger.LogInformation("Запись секций...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись секций выполнена успешно");

            _logger.LogInformation("Запись брендов...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись секций выполнена успешно");

            _logger.LogInformation("Запись товаров...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись товаров выполнена успешно");

            _logger.LogInformation("Запись сотрудников...");
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Employees.AddRange(TestData.Employees);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF");
                await _db.Database.CommitTransactionAsync();
            }
            _logger.LogInformation("Запись сотрудников выполнена успешно");
        }

    }
}
