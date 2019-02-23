using ItemsManager.Common.Types;
using System;

namespace ItemsManager.FoodItems.Commands
{
    public class DeleteFoodItem : ICommand
    {
        public Guid Id { get; }
    }
}
