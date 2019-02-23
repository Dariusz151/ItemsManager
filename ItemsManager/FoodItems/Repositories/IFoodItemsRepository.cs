﻿using ItemsManager.Articles.Domain;
using ItemsManager.FoodItems.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemsManager.FoodItems.Repositories
{
    public interface IFoodItemsRepository
    {
        Task<IEnumerable<FoodItemDTO>> GetAllAsync();
        Task<IEnumerable<FoodItemDTO>> GetAsync(Guid id);
        Task<Guid> CreateAsync(FoodItem foodItem);
        Task<bool> DeleteAsync(Guid id);
    }
}
