using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Models.Interfaces
{
    public interface IRecipesRepository
    {
        Task<IEnumerable<Recipe>> GetAllAsync();
        //Task<IEnumerable<Recipe>> GetAsync(int id);
        //Task<int> CreateAsync(Recipe fridgeItem);
        //Task<bool> DeleteAsync(int id);
        //Task<bool> UpdateAsync(FridgeItem item);
    }
}
