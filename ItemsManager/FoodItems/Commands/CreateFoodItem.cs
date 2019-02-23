using ItemsManager.Common.Types;
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
        public Guid UserId { get; }
    }
}
