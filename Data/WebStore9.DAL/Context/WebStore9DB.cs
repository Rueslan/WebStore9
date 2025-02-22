using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore9Domain.Entities;
using WebStore9Domain.Entities.Identity;
using WebStore9Domain.Entities.Orders;

namespace WebStore9.DAL.Context
{
    public class WebStore9DB : IdentityDbContext<User ,Role, string>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Order> Orders { get; set; }

        //public DbSet<OrderItem> OrdersItems { get; set; } 

        public WebStore9DB(DbContextOptions<WebStore9DB> options) : base(options)
        {

        }
    }
}
