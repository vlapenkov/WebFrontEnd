using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi2
{
    public class ProductsDbContext :DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options):base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
       

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql("Host=localhost;Database=products_db;Username=TNE_USER;Password=123123");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
        .Property(p => p.Id).HasIdentityOptions(startValue: 5);
        

            // в Seed и HasData обязательно указывать ключи:
            //
            //https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding#limitations-of-model-seed-data
            //https://stackoverflow.com/questions/54810281/using-entityframework-core-2-2-to-seed-data-that-has-a-database-generated-key
            modelBuilder.Entity<Product>().HasData(
            new Product{Id=1, Name = "First product" },
            new Product { Id = 2, Name = "Second product" },
            new Product { Id = 3, Name = "Third product" },
            new Product { Id = 4, Name = "Forth product" });
        }
    }
}
