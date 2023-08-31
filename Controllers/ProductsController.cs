using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMPG323_Project2.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace CMPG323_Project2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CMPG323Project2Context _context;

        public ProductsController(CMPG323Project2Context context)
        {
            _context = context;
        }
        //GET: Products
        [HttpGet("Products")]
        public async Task<ActionResult<Product>> Index()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            else
            {
                var products = await _context.Products.ToListAsync();
                return View(products);
            }

        }

        // GET: Products/Details/5
        [HttpGet("Products/Details/{id}")]
        public async Task<ActionResult<Product>> Details(short? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products
        [HttpPost("Products")]
        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new { productId = product.ProductId }, product);
        }

        //PATCH: Products
        [HttpPatch("Product/{id:int}")]
        public IActionResult PatchProduct(short id, [FromBody] JsonPatchDocument<Product> productPatch)
        {
            if (productPatch == null)
            {
                return BadRequest("The JSON Patch document is empty.");
            }
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            productPatch.ApplyTo(product);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(product);
        }

        private bool ProductExists(short id)
        {
            return _context.Products.Any(p => p.ProductId == id);
        }

        //DELETE: Products/5
        [HttpDelete("Products/{id}")]
        public async Task<IActionResult> DeleteProduct(short id)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
