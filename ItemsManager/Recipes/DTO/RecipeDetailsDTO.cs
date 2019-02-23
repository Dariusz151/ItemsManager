using ItemsManager.Recipes.Domain;
using System;
using System.Collections.Generic;

namespace ItemsManager.Recipes.DTO
{
    public class RecipeDetailsDTO
    {
        public string Name { get;  set; }
        public string Description { get;  set; }
        public List<Ingredient> Ingredients { get;  set; }
    }
}
