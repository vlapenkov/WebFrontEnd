using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>The products database context</summary>  
        private readonly ProductsDBContext _productsDbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductsDBContext productsDbContext, ILogger<ProductsController> logger)
        {
            _productsDbContext = productsDbContext;
            _logger = logger;
        }

        /// <summary>Gets the product.</summary>  
        /// <returns>Task<ActionResult<IEnumerable<Product>>>.</returns>  
        /// <remarks> GET api/values</remarks>  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            _logger.LogInformation($"MachineName {Environment.MachineName}");

            var products = await _productsDbContext.Products.ToListAsync();
            var firstProduct = products.First();

            _logger.LogInformation("First product with {id}  is {@Position}", firstProduct.Id, firstProduct);

            return Ok(products);
        }


        [HttpGet]
        public async Task<string> GetProduct2()
        {
            _logger.LogInformation(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            var sqlname = Environment.GetEnvironmentVariable("sqlname")?? "sqlserver";

            var connectionString = $"Server={sqlname};Database=Products;User Id=sa;Password=BigPassw0rd";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.ServerVersion;

            }
            return ""; 
        }

        /// <summary>Creates the specified product.</summary>  
        /// <param name="product">The product.</param>  
        /// <returns>Task<ActionResult<Product>>.</returns>  
        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _productsDbContext.Products.AddAsync(product);
            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        /// <summary>Updates the specified identifier.</summary>  
        /// <param name="id">The identifier.</param>  
        /// <param name="productFromJson">The product from json.</param>  
        /// <returns>Task<ActionResult<Product>>.</returns>  
        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> Update(int id, [FromBody] Product productFromJson)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productsDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Name = productFromJson.Name;
            product.Price = productFromJson.Price;
            product.Description = productFromJson.Description;

            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }

        /// <summary>Deletes the specified identifier.</summary>  
        /// <param name="id">The identifier.</param>  
        /// <returns>Task<ActionResult<Product>>.</returns>  
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            var product = await _productsDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _productsDbContext.Remove(product);
            await _productsDbContext.SaveChangesAsync();

            return Ok(product);
        }
    }
}