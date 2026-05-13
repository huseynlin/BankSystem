using System.Collections.Generic;
using System.Transactions;

namespace BankSystem.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "AZN";

        // User ilə əlaqə
        public int UserId { get; set; }
        public User? User { get; set; }

        // Bu hesaba aid köçürmə tarixçəsi
        public List<Transaction> Transactions { get; set; } = new();
    }
}