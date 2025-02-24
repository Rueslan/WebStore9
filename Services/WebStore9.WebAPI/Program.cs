
using Microsoft.EntityFrameworkCore;
using WebStore9.DAL.Context;

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
