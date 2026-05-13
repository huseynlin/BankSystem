using BankSystem.Data;
using BankSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Services
{
    public class BankDataService
    {
        private readonly AppDbContext _context;

        public BankDataService(AppDbContext context)
        {
            _context = context;
        }

        // --- VALYUTA KONVERTASİYALI TRANSFER + TARİXÇƏ ---
        public bool MakeTransfer(string fromAcc, string toAcc, decimal amount, string targetCurrency)
        {
            var sender = _context.Account.FirstOrDefault(a => a.AccountNumber == fromAcc);
            var receiver = _context.Account.FirstOrDefault(a => a.AccountNumber == toAcc);
            var rates = _context.Rates.ToList();

            if (sender == null || receiver == null || amount <= 0) return false;

            // 1. Göndərilən məbləği AZN-ə çeviririk
            decimal amountInAZN = amount;
            if (targetCurrency != "AZN")
            {
                var rate = rates.FirstOrDefault(r => r.CurrencyCode == targetCurrency);
                if (rate == null) return false;
                amountInAZN = amount * rate.RateToAZN;
            }

            // 2. Göndərənin balansından çıxılacaq məbləğ
            decimal senderDebit = amountInAZN;
            if (sender.Currency != "AZN")
            {
                var sRate = rates.FirstOrDefault(r => r.CurrencyCode == sender.Currency);
                if (sRate == null) return false;
                senderDebit = amountInAZN / sRate.RateToAZN;
            }

            if (sender.Balance < senderDebit) return false;

            // 3. Alanın balansına yatacaq məbləğ
            decimal receiverCredit = amountInAZN;
            if (receiver.Currency != "AZN")
            {
                var rRate = rates.FirstOrDefault(r => r.CurrencyCode == receiver.Currency);
                if (rRate == null) return false;
                receiverCredit = amountInAZN / rRate.RateToAZN;
            }

            // Balansları yenilə
            sender.Balance -= senderDebit;
            receiver.Balance += receiverCredit;

            // --- YENİ: TARİXÇƏYƏ YAZILIR ---
            var transaction = new Transaction
            {
                FromAccountNumber = fromAcc,
                ToAccountNumber = toAcc,
                Amount = amount,
                Currency = targetCurrency,
                TransactionDate = DateTime.Now,
                Description = $"{amount} {targetCurrency} transferi həyata keçirildi.",
                AccountId = sender.Id // Göndərənin hesabına bağlayırıq
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges(); // Bütün dəyişiklikləri bazaya bir dəfəyə yazır
            return true;
        }

        // --- MƏLUMATLARIN GƏTİRİLMƏSİ ---
        public List<User> GetAllUsers() => _context.Users.Include(u => u.Accounts).ToList();
        public List<Account> GetAllAccounts() => _context.Account.Include(a => a.User).ToList();
        public List<ExchangeRate> GetRates() => _context.Rates.ToList();

        // YENİ: Tarixçəni gətirmək üçün
        public List<Transaction> GetTransactionHistory() => _context.Transactions.OrderByDescending(t => t.TransactionDate).ToList();

        // --- ADMİN FUNKSİYALARI ---
        public bool CreateUser(User newUser)
        {
            if (_context.Users.Any(u => u.Email.ToLower() == newUser.Email.ToLower()))
                return false;

            newUser.CreatedAt = DateTime.Now; // Qeydiyyat tarixi
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return true;
        }

        public void CreateAccount(Account newAccount)
        {
            _context.Account.Add(newAccount);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void ToggleUserBlock(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                user.IsBlocked = !user.IsBlocked;
                _context.SaveChanges();
            }
        }

        public void UpdateUser(User updatedUser)
        {
            var user = _context.Users.Find(updatedUser.Id);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;
                if (!string.IsNullOrEmpty(updatedUser.Password))
                    user.Password = updatedUser.Password;

                _context.SaveChanges();
            }
        }

        public void UpdateRate(string code, decimal newRate)
        {
            var rate = _context.Rates.Find(code);
            if (rate != null)
            {
                rate.RateToAZN = newRate;
                _context.SaveChanges();
            }
        }
    }
}