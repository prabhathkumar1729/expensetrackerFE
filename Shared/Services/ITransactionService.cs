using BlazorWalletTrackerFE.Shared.Models;
namespace BlazorWalletTrackerFE.Shared.Services
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactions(int userId);
        Task<Transaction> GetTransactionById(int transactionId);
        Task<Transaction> AddTransaction(Transaction transaction);
        Task<Transaction> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(int transactionId);
        Task<bool> DeleteMultipleTransactions(List<int> transactionIds);
    }
}
