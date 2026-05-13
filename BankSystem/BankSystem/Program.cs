using Microsoft.EntityFrameworkCore;
using BankSystem.Data;
using BankSystem.Services;
using BankSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. SQLite Bağlantı Ayarı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=bank_system.db"));

// 2. Servislərin qeydiyyatı
builder.Services.AddScoped<BankDataService>();
builder.Services.AddControllersWithViews();

// 3. Session ayarları
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// HTTP pipeline tənzimləmələri
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

// --- İLKİN DATALARIN YÜKLƏNMƏSİ (SEED DATA) ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Verilənlər bazasının yarandığından əmin oluruq
    context.Database.EnsureCreated();

    // Əgər istifadəçi cədvəli boşdursa, dataları doldur
    if (!context.Users.Any())
    {
        // 1. Valyutaları yarat
        var rates = new List<ExchangeRate>
        {
            new ExchangeRate { CurrencyCode = "USD", RateToAZN = 1.70m },
            new ExchangeRate { CurrencyCode = "EUR", RateToAZN = 1.85m },
            new ExchangeRate { CurrencyCode = "RUB", RateToAZN = 0.018m }
        };
        context.Rates.AddRange(rates);

        // 2. Admini yarat
        var admin = new User
        {
            FullName = "Baş Admin",
            Email = "admin@mail.com",
            Password = "123",
            Role = "Admin",
            CreatedAt = DateTime.Now
        };
        context.Users.Add(admin);

        // 3. 3 İstifadəçini yarat
        var users = new List<User>
        {
            new User { FullName = "Ali Aliyev", Email = "ali@mail.com", Password = "123", Role = "Customer", CreatedAt = DateTime.Now },
            new User { FullName = "Zeynəb Hüseynli", Email = "zeyneb@mail.com", Password = "123", Role = "Customer", CreatedAt = DateTime.Now },
            new User { FullName = "Elvin Məmmədov", Email = "elvin@mail.com", Password = "123", Role = "Customer", CreatedAt = DateTime.Now }
        };
        context.Users.AddRange(users);

        context.SaveChanges();

        // 4. Test üçün bir hesabı Ali Aliyev-ə bağlayaq (Id-lər bazada yarandıqdan sonra)
        var aliUser = context.Users.FirstOrDefault(u => u.Email == "ali@mail.com");
        if (aliUser != null)
        {
            context.Account.Add(new Account
            {
                AccountNumber = "AZ20261001",
                Balance = 1500.50m,
                Currency = "AZN",
                UserId = aliUser.Id
            });
            context.SaveChanges();
        }
    }
}

app.Run();