using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PonehRestaurantMenu.Data;
using PonehRestaurantMenu.Models;
using System.Linq;

namespace PonehRestaurantMenu.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _db;
        public CustomerController(AppDbContext db) { _db = db; }

        // صفحه انتخاب میز
        public IActionResult SelectTable() => View();

        // شروع یک سفارش جدید برای یک میز و رفتن به منو
        [HttpPost]
        public IActionResult StartOrder(int tableNumber)
        {
            var order = new Order { TableNumber = tableNumber };
            _db.Orders.Add(order);
            _db.SaveChanges();

            return RedirectToAction("Menu", new { orderId = order.Id });
        }

        // نمایش منو برای یک سفارش مشخص (بر اساس OrderId)
        public IActionResult Menu(int orderId)
        {
            var order = _db.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            ViewBag.OrderId = orderId;
            ViewBag.TableNumber = order.TableNumber;

            var items = _db.MenuItems.Where(m => m.IsAvailable).ToList();
            return View(items);
        }

        // افزودن آیتم به سفارش (با OrderId)
        [HttpPost]
        public IActionResult AddToOrder(int orderId, int menuItemId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            var menuItem = _db.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
            if (menuItem == null) return NotFound();

            var existingItem = order.Items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = menuItem.Id,
                    MenuItemName = menuItem.Name,
                    Quantity = 1,
                    Price = menuItem.Price
                };

                order.Items.Add(orderItem);
                _db.OrderItems.Add(orderItem);
            }

            _db.SaveChanges();

            return RedirectToAction("Menu", new { orderId });
        }

        // نمایش سفارش مشتری (بر اساس OrderId)
        public IActionResult MyOrder(int orderId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null) return Content("هیچ سفارشی یافت نشد.");

            return View(order);
        }

        // افزایش تعداد آیتم
        [HttpPost]
        public IActionResult IncreaseQuantity(int orderId, int itemId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            var item = order.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.Quantity++;
                _db.SaveChanges();
            }

            return RedirectToAction("MyOrder", new { orderId });
        }

        // کاهش تعداد آیتم (یا حذف اگر به 1 برسد)
        [HttpPost]
        public IActionResult DecreaseQuantity(int orderId, int itemId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            var item = order.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    order.Items.Remove(item);
                    _db.OrderItems.Remove(item);
                }
                _db.SaveChanges();
            }

            return RedirectToAction("MyOrder", new { orderId });
        }

        // حذف کامل آیتم
        [HttpPost]
        public IActionResult RemoveItem(int orderId, int itemId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null) return NotFound();

            var item = order.Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                order.Items.Remove(item);
                _db.OrderItems.Remove(item);
                _db.SaveChanges();
            }

            return RedirectToAction("MyOrder", new { orderId });
        }

        // رسید سفارش
        public IActionResult Receipt(int orderId)
        {
            var order = _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == orderId);
            if (order == null || !order.Items.Any())
            {
                return Content("هیچ آیتمی در این سفارش ثبت نشده است.");
            }

            return View(order);
        }
    }
}
