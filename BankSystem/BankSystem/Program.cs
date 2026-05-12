var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<BankSystem.Services.BankDataService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Service qeydiyyatı (builder.Build()-dən əvvəl)
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 30 dəqiqə aktiv olmasa çıxış etsin
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// 2. Middleware istifadəsi (app.UseRouting()-dən sonra)
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
