namespace BlazorWalletTrackerFE.Shared.Services
{
    public interface ICategoryService
    {
        Task<List<string>> GetCategories(int userId);
        //Task AddCategory(int userId, string category);
        Task<bool> UpdateCategory(int userId, string oldCategory, string newCategory);
        Task<bool> DeleteAllCategoryTransactions(int userId, string category);
    }
}
