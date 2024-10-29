using MauiApp1.Models;

namespace MauiApp1.Services
{
    public class DataStoreCategories : IDataStore<Category, int>
    {
        readonly List<Category> items;

        public DataStoreCategories()
        {
            items = new List<Category>()
            {
                new Category { Id = 0, Title = "Hobby", Description="Hobby description.", SuccessRate=70 },
                new Category { Id = 1, Title = "Work", Description="Work description.", SuccessRate= 70 },

            };
        }

        public async Task<bool> AddItemAsync(Category item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Category item)
        {
            var oldItem = items.Where((Category arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = items.Where((Category arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Category> GetItemAsync(int id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Category>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
