using System;
using ItemsManager.Common.Types;

namespace ItemsManager.Articles.Domain
{
    public class FoodItem : IIdentifiable, IFoodDetails
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public int Weight { get; private set; }
        public int Quantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public int CategoryId { get; private set; }
        public Guid UserId { get; private set; }

        private FoodItem()
        {

        }

        public FoodItem(Guid id, string name, Guid userId)
        {
            Id = id;
            Name = name;
            Weight = 1;
            Quantity = 1;
            CategoryId = 1;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public FoodItem(Guid id, string name, int weight, int qty, int categoryId, Guid userId)
        {
            Id = id;
            Name = name;
            Weight = weight;
            Quantity = qty;
            CategoryId = categoryId;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

    }
}
