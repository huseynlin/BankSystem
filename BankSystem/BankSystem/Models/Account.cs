namespace BankSystem.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int UserId { get; set; } // User ilə əlaqə
        public string Currency { get; set; } = "AZN";
    }
}