using ItemsManager.Domain;
using ItemsManager.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Models.Interfaces
{
    public interface IRecipesRepository
    {
        Task<IEnumerable<RecipeDTO>> GetAllAsync();
        Task<IEnumerable<RecipeDTO>> GetAsync(int id);
        Task<int> CreateAsync(RecipeDTO recipe);
    }
}
