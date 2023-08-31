using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CMPG323_Project2.Models;
using System.Collections;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.DiaSymReader;
using Microsoft.AspNetCore.Authorization;


namespace CMPG323_Project2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly CMPG323Project2Context _context;

        public CustomersController(CMPG323Project2Context context)
        {
            _context = context;
        }

        //GET: Customers
        [HttpGet("Customers")]
        public async Task<ActionResult<Customer>> Index()
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            else
            {
                var customers = await _context.Customers.ToListAsync();
                return View(customers);
            }

        }

        // GET: Customers/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Details(short? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers
        [HttpPost("Customers")]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new { customerId = customer.CustomerId }, customer);
        }

        //PATCH: Customers
        [HttpPatch("{id:int}")]
        public IActionResult PatchCustomer(short id, [FromBody] JsonPatchDocument<Customer> customerPatch)
        {
            if (customerPatch == null)
            {
                return BadRequest("The JSON Patch document is empty.");
            }
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            customerPatch.ApplyTo(customer);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(customer);
        }

        private bool CustomerExists(short id)
        {
            return _context.Customers.Any(c => c.CustomerId == id);
        }

        //DELETE: Customers/5
        [HttpDelete("Customers/{id}")]
        public async Task<IActionResult> DeleteCustomer(short id)
        {
            if (!CustomerExists(id))
            {
                return NotFound();
            }
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
