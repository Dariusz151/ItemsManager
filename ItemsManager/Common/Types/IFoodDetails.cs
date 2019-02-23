using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Common.Types
{
    public interface IFoodDetails : IDetails
    {
        int Weight { get; }
        int Quantity { get; }
        DateTime CreatedAt { get; }
        int CategoryId { get; }
    }
}
