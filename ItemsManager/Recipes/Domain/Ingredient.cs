namespace ItemsManager.Recipes.Domain
{
    public class Ingredient
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public Ingredient(string name, int weight)
        {
            Name = name;
            Weight = weight;
        }
    }
}
