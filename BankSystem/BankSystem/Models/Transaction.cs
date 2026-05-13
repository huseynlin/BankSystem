using System;

namespace BankSystem.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string FromAccountNumber { get; set; } = string.Empty;
        public string ToAccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;

        // Hesabla əlaqə
        public int AccountId { get; set; }
        public Account? Account { get; set; }
    }
}