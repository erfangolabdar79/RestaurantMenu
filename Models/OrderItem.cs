namespace PonehRestaurantMenu.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        // ارتباط با سفارش
        public int OrderId { get; set; }

        // ارتباط با آیتم منو
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = string.Empty;

        // تعداد و قیمت
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
