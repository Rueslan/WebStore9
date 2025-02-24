using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebStore9.DAL.Context;
using WebStore9.Interfaces.Services;
using WebStore9.Services.Data;
using WebStore9.Services.Services.InCookies;
using WebStore9.Services.Services.InMemory;
using WebStore9.Services.Services.InSQL;
using WebStore9Domain.Entities.Identity;

namespace WebStore9.WebAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var databasetype = builder.Configuration["Database"];

            switch (databasetype)
            {
                default: throw new InvalidOperationException($"Тип БД {databasetype} не поддерживается");

                case "SqlServer":
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseSqlServer(builder.Configuration.GetConnectionString(databasetype)));
                    break;

                case "Sqlite":
                    SQLitePCL.Batteries.Init();
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseSqlite(builder.Configuration.GetConnectionString(databasetype),
                            o => o.MigrationsAssembly("WebStore9.DAL.Sqlite")));
                    break;

                case "InMemory":
                    builder.Services.AddDbContext<WebStore9DB>(opt =>
                        opt.UseInMemoryDatabase("WebStore9.db"));
                    break;
            }

            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStore9DB>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG


                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            });

            builder.Services.AddTransient<WebStore9DBInitializer>();

            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            builder.Services.AddScoped<IProductData, SqlProductData>();
            builder.Services.AddScoped<ICartService, InCookiesCartService>();
            builder.Services.AddScoped<IOrderService, SqlOrderService>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
