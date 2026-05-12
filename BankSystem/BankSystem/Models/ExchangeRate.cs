namespace BankSystem.Models
{
    public class ExchangeRate
    {
        public string CurrencyCode { get; set; } // USD, EUR və s.
        public decimal RateToAZN { get; set; }  // 1 vahidin neçə AZN olduğu (məs: 1.70)
    }
}
