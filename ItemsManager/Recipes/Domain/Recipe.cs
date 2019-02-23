using ItemsManager.Common.Types;
using System;
using System.Collections.Generic;

namespace ItemsManager.Recipes.Domain
{
    public class Recipe : IIdentifiable
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Ingredient> Ingredients { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Recipe(Guid id, string name, string desc, List<Ingredient> ingredients)
        {
            Id = id;
            Name = name;
            Description = desc;
            Ingredients = ingredients;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
