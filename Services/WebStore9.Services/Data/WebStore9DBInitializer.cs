using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using WebStore9.DAL.Context;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.Services.Data
{
    public class WebStore9DBInitializer
    {
        private readonly WebStore9DB _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<WebStore9DBInitializer> _logger;

        public WebStore9DBInitializer(WebStore9DB db,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<WebStore9DBInitializer> Logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = Logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Запуск инициализации БД");
            //var deleted = await _db.Database.EnsureDeletedAsync();
            //var db_created = await _db.Database.EnsureCreatedAsync();

            if (_db.Database.ProviderName.EndsWith("InMemory") || _db.Database.ProviderName.EndsWith("Sqlite"))
                await _db.Database.EnsureCreatedAsync();
            else
            {
                var pending_migrations = await _db.Database.GetPendingMigrationsAsync();
                var applied_migrations = await _db.Database.GetAppliedMigrationsAsync();

                if (pending_migrations.Any())
                {

                    _logger.LogInformation("Применение миграций: {0}", string.Join(",", pending_migrations));
                    await _db.Database.MigrateAsync();
                }
            }

            try
            {
                await InitializeProductsAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка инициализации каталога товаров");
                throw;
            }

            try
            {
                await InitializeIdentityAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка инициализации системы Identity");
                throw;
            }
        }

        private async Task InitializeProductsAsync()
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
                if (product.BrandId is { } brand_id)
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
           
            if (_db.Database.IsSqlServer())
            {
                await using (await _db.Database.BeginTransactionAsync())
                {
                    _db.Sections.AddRange(TestData.Sections);
                    _db.Brands.AddRange(TestData.Brands);
                    _db.Products.AddRange(TestData.Products);
                    _db.Employees.AddRange(TestData.Employees);

                    await _db.SaveChangesAsync();
                    await _db.Database.CommitTransactionAsync();
                }
            }
            else
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);
                _db.Employees.AddRange(TestData.Employees);
                await _db.SaveChangesAsync();
            }

            _logger.LogInformation("Запись данных выполнена успешно за {0} мс", timer.Elapsed.TotalMilliseconds);

        }

        private async Task InitializeIdentityAsync()
        {
            _logger.LogInformation("Инициализация системы Identity");
            var timer = Stopwatch.StartNew();

            //if (!await _roleManager.RoleExistsAsync(Role.Administrators))
            //    await _roleManager.CreateAsync(new Role{Name = Role.Administrators});

            async Task CheckRole(string RoleName)
            {
                if (await _roleManager.RoleExistsAsync(RoleName))
                    _logger.LogInformation($"Роль {RoleName} существует");
                else
                {
                    _logger.LogInformation($"Роль {RoleName} не существует");
                    await _roleManager.CreateAsync(new Role { Name = RoleName });
                    _logger.LogInformation($"Роль {RoleName} успешно создана");
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation($"Пользователь {User.Administrator} не существует");
                var admin = new User
                {
                    UserName = User.Administrator,
                };

                var creation_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь {User.Administrator} успешно создан");

                    await _userManager.AddToRoleAsync(admin, Role.Administrators);

                    _logger.LogInformation($"Пользователю {User.Administrator} успешно добавлена роль {Role.Administrators}");
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description).ToArray();
                    _logger.LogError($"Учётная запись администратора не создана! Ошибки: {string.Join(", ", errors)}");

                    throw new InvalidOperationException($"Невозможно создать администратора {string.Join(", ", errors)}");
                }

                _logger.LogInformation("Инициализация системы Identity выполнена успешно за {0} мс", timer.Elapsed.TotalMilliseconds);
            }

        }

    }
}
