using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorWalletTrackerFE.Shared.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;
        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<string>> GetCategories(int userId)
        {
            var response = await httpClient.GetAsync($"/api/Transaction/GetUserCategories/{userId}");
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<string>>();
                return categories;
            }
            return new List<string>();
        }
        //public async Task AddCategory(int userId, string category)
        //{
        //    await httpClient.PostAsJsonAsync($"api/category/{userId}/{category}", category);
        //}
        public async Task<bool> UpdateCategory(int userId, string oldCategory, string newCategory)
        {
            var custModel = new
            {
                userId = userId,
                oldCategory = oldCategory,
                newCategory = newCategory
            };
            var response = await httpClient.PutAsJsonAsync($"api/Transaction/UpdateUserTransactionsCatergory", custModel);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteAllCategoryTransactions(int userId, string category)
        {
            var response = await httpClient.DeleteAsync($"api/Transaction/DeleteTransactionsHavingCategory/{userId}/{category}");
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
            
        }


    }
}
