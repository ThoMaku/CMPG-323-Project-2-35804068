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
    public class OrdersController : Controller
    {
        private readonly CMPG323Project2Context _context;

        public OrdersController(CMPG323Project2Context context)
        {
            _context = context;
        }

        //GET: Orders
        [HttpGet("Orders")]
        public async Task<ActionResult<Order>> Index()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            else
            {
                var orders = await _context.Orders.ToListAsync();
                return View(orders);
            }

        }

        // GET: Orders/Details/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Details(short? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders
        [HttpPost("Orders")]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Index), new { orderId = order.OrderId }, order);
        }

        //PATCH: Orders
        [HttpPatch("{id:int}")]
        public IActionResult PatchOrder(int id, [FromBody] JsonPatchDocument<Order> orderPatch)
        {
            if (orderPatch == null)
            {
                return BadRequest("The JSON Patch document is empty.");
            }
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            orderPatch.ApplyTo(order);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return View(order);
        }

        private bool OrderExists(int id)
        {
            throw new NotImplementedException();
        }

        //DELETE: Orders/5
        [HttpDelete("Orders")  ]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!OrderExists(id))
            {
                return NotFound();
            }
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("api/Customers/{customerId}/Orders")]
        public IActionResult GetOrdersForCustomer(int customerId)
        {
            var orders = _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToList();

            if (orders == null || orders.Count == 0)
            {
                return NotFound();
            }

            return Ok(orders);
        }
    }
}
