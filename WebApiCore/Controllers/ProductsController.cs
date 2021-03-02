using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Models;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ApplicationContext _context;
        public ProductsController(ApplicationContext context)
        {
            _context = context;
            /*if (!db.Products.Any())
            {
                db.Products.Add(new Product { Name = "Cake", Price = 5 });
                db.Products.Add(new Product { Name = "Candy", Price = 7 });
                db.SaveChanges();
            }*/
        }

        /// <summary>
        /// Получить список всех продуктов
        /// </summary>
        /// <returns>Список всех продуктов</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        /// <summary>
        /// Получить описание продукта по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns>Продукт с указанным идентификатором</returns>
        // GET api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            Product Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (Product == null)
                return NotFound();
            return new ObjectResult(Product);
        }

        // POST api/Products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        // PUT api/Products/
        [HttpPut]
        public async Task<ActionResult<Product>> Put(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!_context.Products.Any(x => x.Id == product.Id))
            {
                return NotFound();
            }

            _context.Update(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            Product Product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(Product);
            await _context.SaveChangesAsync();
            return Ok(Product);
        }
    }
}
