using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFridge.Models
{
    public interface ISmartFridgeRepository
    {
        Task<IEnumerable<FridgeItem>> GetAllAsync();
        Task<IEnumerable<FridgeItem>> GetAsync(int id);
        Task<int> CreateAsync(FridgeItem fridgeItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(FridgeItem item);
    }
}
