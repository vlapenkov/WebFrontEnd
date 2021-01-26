using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApi2.Contracts.Dto;
using WebApi2.Contracts.Intrerfaces;

namespace WebApi2.Web
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy ="DefaultPolicy")]
    public class ProductsController : ControllerBase, IProductService
    {
        /// <summary>The products database context</summary>  
        private readonly ProductsDbContext _productsDbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductsDbContext productsDbContext, ILogger<ProductsController> logger)
        {
            _productsDbContext = productsDbContext;
            _logger = logger;
        }

        /// <summary>Gets the product.</summary>  
        /// <returns>Task<ActionResult<IEnumerable<Product>>>.</returns>  
        /// <remarks> GET api/values</remarks>  
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var token =await HttpContext.GetTokenAsync("access_token");
            _logger.LogInformation($"MachineName {Environment.MachineName}");

            var products = await _productsDbContext.Products.Select(p=>new ProductDto {Id=p.Id,Name=p.Name }).OrderBy(p=>p.Name).ToListAsync();
            var firstProduct = products.First();

            _logger.LogInformation("First product with {id}  is {@Position}", firstProduct.Id, firstProduct);



            return products;
        }


        //[HttpGet]
        //public async Task<string> GetProduct2()
        //{
        //    _logger.LogInformation(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
        //    var sqlname = Environment.GetEnvironmentVariable("sqlname")?? "sqlserver";

        //    var connectionString = $"Server={sqlname};Database=Products;User Id=sa;Password=BigPassw0rd";

        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        return connection.ServerVersion;

        //    }
        //    return ""; 
        //}

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