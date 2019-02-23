using ItemsManager.Recipes.Domain;
using ItemsManager.Recipes.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Repositories
{
    public interface IRecipesRepository
    {
        Task<IEnumerable<RecipeDTO>> GetAllAsync();
        Task<RecipeDetailsDTO> GetAsync(Guid id);
        Task<Guid> CreateAsync(Recipe recipe);

        Task<IEnumerable<RecipeDetailsDTO>> GetAllRecipesAsync();
    }
}
