namespace BlazorWalletTrackerFE.Shared.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }

        public DateTime? TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Category { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
