using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Infrastructure
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees{get; set; }
        public DbSet<Product> Products{get; set; }
        public DbSet<Category> Categories{get; set; }
        public DbSet<CartItem> CartItems{get; set; }
        public DbSet<Cart> Carts{get;set;}
        public DbSet<Customer> Customers{get; set; }
    }
}