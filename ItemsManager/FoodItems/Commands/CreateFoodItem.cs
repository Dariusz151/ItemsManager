using ItemsManager.Common.Types;
using Newtonsoft.Json;
using System;

namespace ItemsManager.FoodItems.Commands
{
    //immutable
    public class CreateFoodItem : ICommand
    {
        public string Name { get; }
        public int Weight { get; }
        public int Quantity { get; }
        public int CategoryId { get; }

        [JsonConstructor]
        public CreateFoodItem(string name, int weight, int quantity, int categoryId)
        {
            Name = name;
            Weight = weight;
            Quantity = quantity;
            CategoryId = categoryId;
        }
    }
}
