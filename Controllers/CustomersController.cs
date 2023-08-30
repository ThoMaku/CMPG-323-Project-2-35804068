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

namespace CMPG323_Project2.Controllers
{
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
        [HttpGet]
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
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new {customerId = customer.CustomerId}, customer);
        }

        //PATCH: Customers
        [HttpPatch("{id:int}")]
        public IActionResult PatchCustomer(int id, [FromBody] JsonPatchDocument<Customer> customerPatch)
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
            catch(DbUpdateConcurrencyException)
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

        private bool CustomerExists(int id)
        {
            throw new NotImplementedException();
        }

        //DELETE: Customers/5
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(int id)
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
