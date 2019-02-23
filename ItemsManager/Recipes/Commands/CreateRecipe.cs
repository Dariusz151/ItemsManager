using ItemsManager.Common.Types;
using ItemsManager.Recipes.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ItemsManager.Recipes.Commands
{
    public class CreateRecipe : ICommand
    {
        //immutable
        public string Name { get; }
        public string Description { get;}
        public List<Ingredient> Ingredients { get; }

        [JsonConstructor]
        public CreateRecipe(string name, string desc, List<Ingredient> ingredients)
        {
            Name = name;
            Description = desc;
            Ingredients = ingredients;
        }
    }
}
