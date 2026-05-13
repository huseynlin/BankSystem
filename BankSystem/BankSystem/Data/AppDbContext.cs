using Microsoft.EntityFrameworkCore;
using BankSystem.Models;

namespace BankSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Cədvəllərimiz
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Account { get; set; } // Qeyd: Bəzən plural (Accounts) istifadə olunur, modelə uyğun saxladım
        public DbSet<ExchangeRate> Rates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Valyuta kodu Primary Key olsun
            modelBuilder.Entity<ExchangeRate>().HasKey(r => r.CurrencyCode);

            // Account və User arasındakı əlaqə
            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId);

            // Account və Transaction arasındakı əlaqə
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            base.OnModelCreating(modelBuilder);
        }
    }
}