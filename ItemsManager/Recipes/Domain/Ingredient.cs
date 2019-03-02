using Newtonsoft.Json;

namespace ItemsManager.Recipes.Domain
{
    public class Ingredient
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        //[JsonConstructor]
        //public Ingredient(string name, int weight)
        //{
        //    Name = name;
        //    Weight = weight;
        //}
    }
}
