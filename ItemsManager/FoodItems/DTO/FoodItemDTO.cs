using System;

namespace ItemsManager.FoodItems.DTO
{
    public class FoodItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Quantity { get; set; }
    }
}
