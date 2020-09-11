using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>The products database context</summary>  
        private readonly ProductsDBContext _productsDbContext;

        public ProductsController(ProductsDBContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        /// <summary>Gets the product.</summary>  
        /// <returns>Task<ActionResult<IEnumerable<Product>>>.</returns>  
        /// <remarks> GET api/values</remarks>  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            Console.WriteLine($"MachineName {Environment.MachineName}");
            return Ok(await _productsDbContext.Products.ToListAsync());
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