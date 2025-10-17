using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PonehRestaurantMenu.Data;

namespace PonehRestaurantMenu.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        public AdminController(AppDbContext db) { _db = db; }

        public IActionResult Orders(int? tableNumber)
        {
            var query = _db.Orders.Include(o => o.Items).AsQueryable();

            if (tableNumber.HasValue)
                query = query.Where(o => o.TableNumber == tableNumber.Value);

            var orders = query
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return View(orders);
        }
    }
}
