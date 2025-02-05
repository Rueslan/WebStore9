using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;

namespace WebStore9.Data
{
    public class WebStore9DBInitializer
    {
        private readonly WebStore9DB _db;

        public WebStore9DBInitializer(WebStore9DB db)
        {
            _db = db;
        }

        public async Task InitializeAsync()
        {
            //var deleted = await _db.Database.EnsureDeletedAsync();
            //var db_created = await _db.Database.EnsureCreatedAsync();

            var pending_migrations = await _db.Database.GetAppliedMigrationsAsync();
            var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();
            
            if (pending_migrations.Any())
                await _db.Database.MigrateAsync();

            await InitializeProductsAsync();
        }

        public async Task InitializeProductsAsync()
        {
            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Sections.AddRange(TestData.Sections);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                await _db.Database.CommitTransactionAsync();
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Brands.AddRange(TestData.Brands);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                await _db.Database.CommitTransactionAsync();
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Products.AddRange(TestData.Products);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");
                await _db.SaveChangesAsync();
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
                await _db.Database.CommitTransactionAsync();
            }

            await using (await _db.Database.BeginTransactionAsync())
            {
                _db.Employees.AddRange(TestData.Employees);

                await _db.SaveChangesAsync();
                await _db.Database.CommitTransactionAsync();
            }
        }

    }
}
