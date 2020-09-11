using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApi
{
    public class ProductsDBContext : DbContext
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="ProductsDBContext"/> class.  
        /// </summary>  
        /// <param name="options">The options.</param>  
        public ProductsDBContext(DbContextOptions<ProductsDBContext> options) : base(options)
        {
          
        }

        /// <summary>  
        /// Gets or sets the products.  
        /// </summary>  
        /// <value>The products.</value>  
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
                new Product[] { 
                new Product {Id=1,Name="First",Price=10,Description=1 },
                new Product {Id=2,Name="Second",Price=20,Description=1 },
                new Product {Id=3,Name="Third",Price=30,Description=1 }
                }
                );
        }
    }
}
