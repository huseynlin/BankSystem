using System.ComponentModel.DataAnnotations;

namespace BankSystem.Models
{
    public class ExchangeRate
    {
        [Key] // Primary Key olaraq təyin edirik
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal RateToAZN { get; set; }
    }
}