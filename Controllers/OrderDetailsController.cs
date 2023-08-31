using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMPG323_Project2.Models;
using Microsoft.AspNetCore.Authorization;


namespace CMPG323_Project2.Controllers
{
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly CMPG323Project2Context _context;

        public OrderDetailsController(CMPG323Project2Context context)
        {
            _context = context;
        }

        [HttpGet("api/Order/{orderId}/Products")]
        public IActionResult GetProductsForOrders(int orderId)
        {
            var products = _context.OrderDetails
                .Where(p => p.OrderId == orderId)
                .ToList();

            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return Ok(products);
        }
    }
}
