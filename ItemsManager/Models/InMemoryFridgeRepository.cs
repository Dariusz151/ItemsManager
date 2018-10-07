using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SmartFridge.Models
{
    public class InMemoryFridgeRepository : ISmartFridgeRepository
    {
        public static DateTime dt = new DateTime();

        private static List<FridgeItem> _fridgeItemList = new List<FridgeItem>()
        {
            //todo update 

            //new FridgeItem
            //{
            //    ID = 1,  ArticleName = "Milk", InsertDate = dt, ExpirationDate = dt, Quantity = 1, Weight = "1000ml"
            //},
            //new FridgeItem
            //{
            //    ID = 2,  ArticleName = "Apple", InsertDate = dt, ExpirationDate = dt, Quantity = 1, Weight = "Unknown"
            //},new FridgeItem
            //{
            //    ID = 3,  ArticleName = "Yoghurt", InsertDate = dt, ExpirationDate = dt, Quantity = 1, Weight = "200ml"
            //}
        };

        public Task<int> CreateAsync(FridgeItem fridgeItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FridgeItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FridgeItem>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(FridgeItem item)
        {
            throw new NotImplementedException();
        }

        //    private static int _nextId = 4;

        //    public async Task<IEnumerable<FridgeItem>> GetAllAsync()
        //    {
        //        if (!_fridgeItemList.Any())
        //            return null;
        //        return await Task.FromResult(_fridgeItemList);
        //    }

        //    public async Task<FridgeItem> GetAsync(int id)
        //    {
        //        return await Task.FromResult(_fridgeItemList.SingleOrDefault(x => x.ID == id));
        //    }

        //    public async Task<int> CreateAsync(FridgeItem fridgeItem)
        //    {
        //        var newFridgeItem = new FridgeItem { ArticleName = fridgeItem.ArticleName, ExpirationDate = fridgeItem.ExpirationDate, InsertDate = fridgeItem.InsertDate, Quantity = fridgeItem.Quantity, Weight = fridgeItem.Weight, ID = _nextId++ };
        //        _fridgeItemList.Add(newFridgeItem);

        //        return await Task.FromResult(newFridgeItem.ID);
        //    }

        //    public async Task<bool> DeleteAsync(int id)
        //    {
        //        try
        //        {
        //            var deleted = _fridgeItemList.SingleOrDefault(x => x.ID == id);
        //            _fridgeItemList.Remove(deleted);
        //            deleted.ID = 0;
        //        }
        //        catch
        //        {
        //            return await Task.FromResult(false);
        //        }
        //        return await Task.FromResult(true);
        //    }
    }
}
