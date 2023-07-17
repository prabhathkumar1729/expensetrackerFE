using BlazorWalletTrackerFE.Shared.Models;
using BlazorWalletTrackerFE.Shared.Services;
using Fluxor;

namespace BlazorWalletTrackerFE.Store.TransactionStore
{
    public class TransactionState
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public bool IsLoading { get; set; } = false;
        public string Error { get; set; } = null;


    }

    public class TransactionFeature : Feature<TransactionState>
    {
        public override string GetName() => "Transaction";

        protected override TransactionState GetInitialState()
        {
            return new TransactionState();
        }
    }
    public class FetchTransactionsAction
    {
        public int UserId { get; set; }
    }
    public class FetchTransactionsSuccessAction
    {
        public List<Transaction> Transactions { get; set; }
    }

    public class AddTransactionAction
    {
        public Transaction Transaction { get; set; }
    }
    public class SaveNewTransactionAction
    {
        public Transaction Transaction { get; set; }
    }
    public class UpdateTransactionAction
    {
        public Transaction Transaction { get; set; }
    }
    public class SetUpdatedTransactionAction
    {
        public Transaction Transaction { get; set; }
    }
    public class DeleteTransactionAction
    {
        public int TransactionId { get; set; }
    }

    public class DeleteTransactionHavingIdAction
    {
        public int TransactionId { get; set; }
    }

    public class DeleteSelectedTransactionsAction
    {
        public List<int> TransactionIds { get; set; }
    }

    public class DeleteMultipleTransactionsFromStoreAction
    {
        public List<int> TransactionIds { get; set; }
    }
    public class UpdateCategoryInTransactionsAction
    {
        public int UserId { get; set; }
        public string OldCategory { get; set; }
        public string NewCategory { get; set; }
    }

    public class DeleteAllTransactionsHavingCategoryAction
    {
        public int UserId { get; set; }
        public string Category { get; set; }
    }

    //write action and reducers to clear all transactions state storage
    public class ClearTransactionsStateAction
    {
    }
    public class TransactionReducers
    {
        [ReducerMethod]
        public static TransactionState ReduceFetchTransactions(TransactionState state, FetchTransactionsAction action)
        {
            return new TransactionState { IsLoading = true };
        }

        [ReducerMethod]
        public static TransactionState ReduceFetchTransactionsSuccess(TransactionState state, FetchTransactionsSuccessAction action)
        {
            return new TransactionState { Transactions = action.Transactions, IsLoading = false };
        }

        [ReducerMethod]
        public static TransactionState ReduceSaveNewTransaction(TransactionState state, SaveNewTransactionAction action)
        {
            var updatedTransactions = state.Transactions.ToList();
            updatedTransactions.Add(action.Transaction);
            return new TransactionState { Transactions = updatedTransactions };
        }

        [ReducerMethod]
        public static TransactionState ReduceUpdateTransaction(TransactionState state, SetUpdatedTransactionAction action)
        {
            var updatedTransactions = state.Transactions.ToList();
            var existingTransaction = updatedTransactions.FirstOrDefault(t => t.TransactionId == action.Transaction.TransactionId);
            if (existingTransaction != null)
            {
                // Update the existing transaction properties with the new values
                existingTransaction.UserId = action.Transaction.UserId;
                existingTransaction.TransactionDate = action.Transaction.TransactionDate;
                existingTransaction.Amount = action.Transaction.Amount;
                existingTransaction.Category = action.Transaction.Category;
                existingTransaction.Description = action.Transaction.Description;
            }
            return new TransactionState { Transactions = updatedTransactions };
        }

        [ReducerMethod]
        public static TransactionState ReduceDeleteTransactionHavingId(TransactionState state, DeleteTransactionHavingIdAction action)
        {
            var updatedTransactions = state.Transactions.Where(t => t.TransactionId != action.TransactionId).ToList();
            return new TransactionState { Transactions = updatedTransactions };
        }

        [ReducerMethod]
        public static TransactionState ReduceUpdateCategoryInTransactions(TransactionState state, UpdateCategoryInTransactionsAction action)
        {
            var updatedTransactions = state.Transactions
                        .Select(t => t.Category == action.OldCategory
                            ? new Shared.Models.Transaction { TransactionId = t.TransactionId, Amount = t.Amount, Category = action.NewCategory, Description = t.Description, TransactionDate = t.TransactionDate, UserId = t.UserId }
                            : t)
                        .ToList();

            return new TransactionState { Transactions = updatedTransactions };


        }

        [ReducerMethod]
        public static TransactionState ReduceDeleteAllTransactionsHavingCategory(TransactionState state, DeleteAllTransactionsHavingCategoryAction action)
        {
            var updatedTransactions = state.Transactions.Where(t => t.Category != action.Category).ToList();
            return new TransactionState { Transactions = updatedTransactions };
        }

        [ReducerMethod]
        public static TransactionState ReduceDeleteMultipleTransactionsFromStore(TransactionState state, DeleteMultipleTransactionsFromStoreAction action)
        {
            var updatedTransactions = state.Transactions.Where(t => !action.TransactionIds.Contains(t.TransactionId)).ToList();
            return new TransactionState { Transactions = updatedTransactions };
        }

        [ReducerMethod]
        public static TransactionState ReduceClearTransactionsState(TransactionState state, ClearTransactionsStateAction action)
        {
            return new TransactionState();
        }

    }

    public class TransactionEffects
    {
        private readonly ITransactionService _transactionService;

        public TransactionEffects(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [EffectMethod]
        public async Task HandleFetchTransactions(FetchTransactionsAction action, IDispatcher dispatcher)
        {
            try
            {
                var transactions = await _transactionService.GetTransactions(action.UserId); // Make API call to fetch transactions
                dispatcher.Dispatch(new FetchTransactionsSuccessAction { Transactions = transactions });
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        [EffectMethod]
        public async Task HandleAddTransaction(AddTransactionAction action, IDispatcher dispatcher)
        {
            try
            {
                var addedTransaction = await _transactionService.AddTransaction(action.Transaction); // Make API call to add transaction
                dispatcher.Dispatch(new SaveNewTransactionAction { Transaction = addedTransaction });
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
        [EffectMethod]
        public async Task HandleUpdateTransaction(UpdateTransactionAction action, IDispatcher dispatcher)
        {
            try
            {
                Transaction resTransaction = await _transactionService.UpdateTransaction(action.Transaction); // Make API call to update transaction
                //dispatcher.Dispatch(new UpdateTransactionAction { Transaction = action.Transaction });
                dispatcher.Dispatch(new SetUpdatedTransactionAction { Transaction = resTransaction });
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }

        [EffectMethod]
        public async Task HandleDeleteTransaction(DeleteTransactionAction action, IDispatcher dispatcher)
        {
            try
            {
                if (await _transactionService.DeleteTransaction(action.TransactionId)) // Make API call to delete transaction
                {
                    dispatcher.Dispatch(new DeleteTransactionHavingIdAction { TransactionId = action.TransactionId });
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
        [EffectMethod]
        public async Task HandleDeleteSelectedTransactions(DeleteSelectedTransactionsAction action, IDispatcher dispatcher)
        {
            try
            {
                if (await _transactionService.DeleteMultipleTransactions(action.TransactionIds)) // Make API call to delete transaction
                {
                    dispatcher.Dispatch(new DeleteMultipleTransactionsFromStoreAction { TransactionIds = action.TransactionIds });
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }

}
