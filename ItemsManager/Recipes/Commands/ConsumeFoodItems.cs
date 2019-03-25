using ItemsManager.Common.Types;
using Newtonsoft.Json;
using System;

namespace ItemsManager.Recipes.Commands
{
    public class ConsumeFoodItems : ICommand
    {
        //immutable
        public Guid RecipeId { get; private set; }
       
        [JsonConstructor]
        public ConsumeFoodItems(Guid id)
        {
            this.RecipeId = id;
        }
    }
}
