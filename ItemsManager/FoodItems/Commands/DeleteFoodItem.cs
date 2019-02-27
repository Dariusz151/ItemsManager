using ItemsManager.Common.Types;
using Newtonsoft.Json;
using System;

namespace ItemsManager.FoodItems.Commands
{
    public class DeleteFoodItem : ICommand
    {
        public Guid Id { get; }

        [JsonConstructor]
        public DeleteFoodItem(Guid id)
        {
            Id = id;
        }
    }
}
