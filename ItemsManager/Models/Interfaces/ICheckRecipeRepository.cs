using ItemsManager.Domain;
using ItemsManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Models.Interfaces
{
    public interface ICheckRecipeRepository
    {
        Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync();
    }
}
