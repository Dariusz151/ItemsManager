using ItemsManager.FoodItems.Domain.Models;
using ItemsManager.FoodItems.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemsManager.FoodItems.Domain.Repositories
{
    public interface IFoodItemsRepository
    {
        Task<IEnumerable<FoodItemDTO>> GetAllAsync();
        Task<IEnumerable<FoodItemDTO>> GetAsync(Guid id);
        Task<bool> CreateAsync(FoodItem foodItem);
        Task<bool> DeleteAsync(Guid id);
    }
}
