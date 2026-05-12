using System.Text.Json;
using BankSystem.Models;

namespace BankSystem.Services
{
    public class BankDataService
    {
        private readonly string _filePath = "database.json";

        public class BankData
        {
            public List<User> Users { get; set; } = new();
            public List<Account> Accounts { get; set; } = new();
            public List<ExchangeRate> Rates { get; set; } = new();
        }

        private BankData LoadData()
        {
            try
            {
                if (!File.Exists(_filePath)) return new BankData();
                var json = File.ReadAllText(_filePath);
                if (string.IsNullOrWhiteSpace(json)) return new BankData();
                return JsonSerializer.Deserialize<BankData>(json) ?? new BankData();
            }
            catch (Exception) { return new BankData(); }
        }

        private void SaveData(BankData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
        }

        public bool MakeTransfer(string fromAcc, string toAcc, decimal amount, string targetCurrency)
        {
            var data = LoadData();
            var sender = data.Accounts.FirstOrDefault(a => a.AccountNumber == fromAcc);
            var receiver = data.Accounts.FirstOrDefault(a => a.AccountNumber == toAcc);

            if (sender == null || receiver == null || amount <= 0) return false;

            decimal amountInAZN = amount;
            if (targetCurrency != "AZN")
            {
                var rate = data.Rates.FirstOrDefault(r => r.CurrencyCode == targetCurrency);
                if (rate == null) return false;
                amountInAZN = amount * rate.RateToAZN;
            }

            decimal senderDebit = amountInAZN;
            if (sender.Currency != "AZN")
            {
                var sRate = data.Rates.FirstOrDefault(r => r.CurrencyCode == sender.Currency);
                if (sRate == null) return false;
                senderDebit = amountInAZN / sRate.RateToAZN;
            }

            if (sender.Balance < senderDebit) return false;

            decimal receiverCredit = amountInAZN;
            if (receiver.Currency != "AZN")
            {
                var rRate = data.Rates.FirstOrDefault(r => r.CurrencyCode == receiver.Currency);
                if (rRate == null) return false;
                receiverCredit = amountInAZN / rRate.RateToAZN;
            }

            sender.Balance -= senderDebit;
            receiver.Balance += receiverCredit;

            SaveData(data);
            return true;
        }

        public List<Account> GetAllAccounts() => LoadData().Accounts;
        public List<User> GetAllUsers() => LoadData().Users;
        public List<ExchangeRate> GetRates() => LoadData().Rates;

        // --- DÜZƏLİŞ EDİLDİ: EMAİL YOXLAMASI ƏLAVƏ OLUNDU ---
        public bool CreateUser(User newUser)
        {
            var data = LoadData();

            // Email unikal olmalıdır (Böyük-kiçik hərf fərqi olmadan yoxlayırıq)
            if (data.Users.Any(u => u.Email.ToLower() == newUser.Email.ToLower()))
            {
                return false; // Email artıq var
            }

            newUser.Id = data.Users.Any() ? data.Users.Max(u => u.Id) + 1 : 1;
            data.Users.Add(newUser);
            SaveData(data);
            return true; // Uğurlu yaradılma
        }

        public void CreateAccount(Account newAccount)
        {
            var data = LoadData();
            newAccount.Id = data.Accounts.Any() ? data.Accounts.Max(a => a.Id) + 1 : 1;
            data.Accounts.Add(newAccount);
            SaveData(data);
        }

        public void DeleteUser(int userId)
        {
            var data = LoadData();
            data.Users.RemoveAll(u => u.Id == userId);
            data.Accounts.RemoveAll(a => a.UserId == userId);
            SaveData(data);
        }

        public void ToggleUserBlock(int userId)
        {
            var data = LoadData();
            var user = data.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null) { user.IsBlocked = !user.IsBlocked; SaveData(data); }
        }

        public void UpdateUser(User updatedUser)
        {
            var data = LoadData();
            var user = data.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.Role = updatedUser.Role;
                if (!string.IsNullOrEmpty(updatedUser.Password)) user.Password = updatedUser.Password;
                SaveData(data);
            }
        }

        public void UpdateRate(string code, decimal newRate)
        {
            var data = LoadData();
            var rate = data.Rates.FirstOrDefault(r => r.CurrencyCode == code);
            if (rate != null) { rate.RateToAZN = newRate; SaveData(data); }
        }
    }
}