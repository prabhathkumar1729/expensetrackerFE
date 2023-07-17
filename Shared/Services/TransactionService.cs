using System.Net.Http.Json;
using BlazorWalletTrackerFE.Shared.Models;

namespace BlazorWalletTrackerFE.Shared.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Transaction>> GetTransactions(int userId)
        {
            // Make API call to retrieve transactions
            var response = await _httpClient.GetAsync("/api/Transaction/GetAllTransactions/"+userId);
            response.EnsureSuccessStatusCode();
            var transactions = await response.Content.ReadFromJsonAsync<List<Transaction>>();
            return transactions;
        }

        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            // Make API call to retrieve transaction by ID
            var response = await _httpClient.GetAsync($"api/Transaction/GetTransaction/{transactionId}");
            response.EnsureSuccessStatusCode();
            var transaction = await response.Content.ReadFromJsonAsync<Transaction>();
            return transaction;
        }

        public async Task<Transaction> AddTransaction(Transaction transaction)
        {
            // Make API call to add transaction
            var response = await _httpClient.PostAsJsonAsync("api/Transaction/AddTransaction", transaction);
            response.EnsureSuccessStatusCode();
            var addedTransaction = await response.Content.ReadFromJsonAsync<Transaction>();
            return addedTransaction;
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            // Make API call to update transaction
            var response = await _httpClient.PutAsJsonAsync($"api/Transaction/UpdateTransaction", transaction);
            response.EnsureSuccessStatusCode();
            var updatedTransaction = await response.Content.ReadFromJsonAsync<Transaction>();
            return updatedTransaction;
        }

        public async Task<bool> DeleteTransaction(int transactionId)
        {
            // Make API call to delete transaction
            Console.WriteLine("received transaction id",transactionId);
            var response = await _httpClient.DeleteAsync($"api/Transaction/DeleteTransaction/{transactionId}");
            return response.IsSuccessStatusCode;
            
        }

        public async Task<bool> DeleteMultipleTransactions(List<int> transactionIds)
        {
            // Make API call to delete multiple transactions
            var response = await _httpClient.PostAsJsonAsync("api/Transaction/DeleteMultipleTransactions", transactionIds);
            return response.IsSuccessStatusCode;
        }
    }
}
