using System;
using System.Collections.Generic;
using System.Linq;
using ItemsManager.Domain;
using ItemsManager.Models.DTO;

namespace ItemsManager.Utilities
{
    public class MatchIngredients
    {
        public static IEnumerable<RecipeDTO> Match(IEnumerable<RecipeDTO> recipes, List<Ingredient> list)
        {
            List<RecipeDTO> matchedList = new List<RecipeDTO>();
            List<RecipeDTO> filteredMatchedList = new List<RecipeDTO>();

            List<string> matchedListNames = new List<string>();
            List<string> articlesFromUser = new List<string>();
            List<string> articlesFromRecipes = new List<string>();
            bool notEnough = false;
           
            foreach (var el in list)
            {
                articlesFromUser.Add(el.Name);
            }

            foreach (var recipe in recipes)
            {
                articlesFromRecipes.Clear();
                foreach (var ingr in recipe.Ingredients)
                {
                    articlesFromRecipes.Add(ingr.Name);
                }
                bool isSubset = !articlesFromRecipes.Except(articlesFromUser).Any();
               
                if (isSubset)
                {
                    matchedList.Add(recipe);
                }
            }
            
            for (int i = 0; i < matchedList.Count; i++)
            {
                var recipe = matchedList[i];
                notEnough = false;

                for (int j = 0; j < recipe.Ingredients.Count; j++)
                {
                    var ingr = recipe.Ingredients[j];
                    var recipeIngrWeight = ingr.Weight;
                    var userIngrWeight = list.Where(x => x.Name == ingr.Name).FirstOrDefault().Weight;

                    if (userIngrWeight < recipeIngrWeight)
                    {
                        notEnough = true;
                    }
                }
                if (notEnough == false)
                {
                    filteredMatchedList.Add(recipe);
                }
            }
            
            return filteredMatchedList.OrderByDescending(x=> x.Ingredients.Count);
        }
    }
}
