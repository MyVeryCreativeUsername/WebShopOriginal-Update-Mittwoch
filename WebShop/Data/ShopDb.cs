using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebShop.Models.Mappings;
using WebShop.Models.ShopEntities;
using WebShop.Models.UserEntities;

namespace WebShop.Data
{
    public class ShopDb : DbContext
    {

        private readonly IConfiguration _config;

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductsOrdersMapping> ProductsOrdersMapping { get; set; }
        public DbSet<ProductsCategoriesMapping> ProductsCategoriesMapping { get; set; }

        public ShopDb(IConfiguration config, DbContextOptions<ShopDb> options) : base(options)
        {
            _config = config;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseSqlite(_config.GetConnectionString("DefaultConnection"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(e => e.Categorys)
                .WithMany(e => e.Products)
                .UsingEntity<ProductsCategoriesMapping>(
                    l => l.HasOne<Category>(e => e.Category).WithMany(e => e.ProductCategoriesMapping),
                    r => r.HasOne<Product>(e => e.Product).WithMany(e => e.ProductCategoriesMapping));

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Orders)
                .WithMany(e => e.Products)
                .UsingEntity<ProductsOrdersMapping>(
                    l => l.HasOne<Order>(e => e.Order).WithMany(e => e.ProductsOrdersMapping),
                    r => r.HasOne<Product>(e => e.Product).WithMany(e => e.ProductsOrdersMapping));



        }
    }
}
