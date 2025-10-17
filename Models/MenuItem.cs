namespace PonehRestaurantMenu.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
