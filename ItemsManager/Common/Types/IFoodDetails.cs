using ItemsManager.FoodItems.Types;
using System;

namespace ItemsManager.Common.Types
{
    public interface IFoodDetails : IDetails
    {
        int Weight { get; }
        int Quantity { get; }
        DateTime CreatedAt { get; }
        FoodCategory CategoryId { get; }
    }
}
