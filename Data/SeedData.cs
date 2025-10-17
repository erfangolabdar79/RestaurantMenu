using PonehRestaurantMenu.Models;

namespace PonehRestaurantMenu.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext db)
        {
            if (!db.MenuItems.Any())
            {
                db.MenuItems.AddRange(
                    new MenuItem { Category = "غذا", Name = "کباب کوبیده", Price = 120000, IsAvailable = true },
                    new MenuItem { Category = "غذا", Name = "جوجه کباب", Price = 110000, IsAvailable = true },
                    new MenuItem { Category = "نوشیدنی", Name = "دوغ", Price = 20000, IsAvailable = true },
                    new MenuItem { Category = "نوشیدنی", Name = "نوشابه", Price = 15000, IsAvailable = true }
                );
                db.SaveChanges();
            }
        }
    }
}
