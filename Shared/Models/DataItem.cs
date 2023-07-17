namespace BlazorWalletTrackerFE.Shared.Models
{
    public class DataItem
    {
        public string Category { get; set; }
        public string Month { get; set; }
        public double TotalSpendingsInTheMonth { get; set; }
        public int TotalNumberOfTransactionsInTheMonth { get; set; }
    }
}