using Microsoft.AspNetCore.Mvc;
using BankSystem.Services;
using BankSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace BankSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly BankDataService _bankService;

        public AdminController(BankDataService bankService)
        {
            _bankService = bankService;
        }

        // Admin Ana Səhifəsi
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Auth");

            var users = _bankService.GetAllUsers();
            return View(users);
        }

        // Yeni İstifadəçi Yaratma (Səhifə)
        [HttpGet]
        public IActionResult CreateUser() => View();

        // Yeni İstifadəçi Yaratma (Məlumatı bazaya yazma)
        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            // Servis bool qaytardığı üçün nəticəni yoxlayırıq
            bool isCreated = _bankService.CreateUser(user);

            if (!isCreated)
            {
                // Email artıq mövcuddursa bu blok işləyir
                TempData["Error"] = "Bu email artıq istifadə olunub! Zəhmət olmasa başqa email daxil edin.";
                return View(user); // Admin məlumatları yenidən görməsi üçün View-ya qaytarırıq
            }

            TempData["Success"] = "İstifadəçi uğurla yaradıldı.";
            return RedirectToAction("Index");
        }

        // Redaktə Etmə (Səhifə)
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _bankService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        // Redaktə Etmə (Yadda saxlama)
        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            _bankService.UpdateUser(user);
            TempData["Success"] = "Məlumatlar yeniləndi.";
            return RedirectToAction("Index");
        }

        // Hesab Açma
        [HttpPost]
        public IActionResult CreateAccount(int userId, string currency)
        {
            var newAcc = new Account
            {
                UserId = userId,
                AccountNumber = "AZ" + new Random().Next(100000, 999999),
                Balance = 0,
                Currency = currency
            };
            _bankService.CreateAccount(newAcc);
            TempData["Success"] = "Hesab açıldı.";
            return RedirectToAction("Index");
        }

        // İstifadəçini Silmə
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            _bankService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        // Bloklama
        [HttpPost]
        public IActionResult ToggleBlock(int id)
        {
            _bankService.ToggleUserBlock(id);
            return RedirectToAction("Index");
        }

        // Məzənnə Səhifəsi
        public IActionResult Rates()
        {
            var rates = _bankService.GetRates();
            return View(rates);
        }

        [HttpPost]
        public IActionResult UpdateRate(string code, string rate)
        {
            string normalizedRate = rate.Replace(",", ".");

            if (decimal.TryParse(normalizedRate, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal finalRate))
            {
                _bankService.UpdateRate(code, finalRate);
                TempData["Success"] = $"{code} məzənnəsi {finalRate} olaraq yeniləndi.";
            }
            else
            {
                TempData["Error"] = "Məzənnə formatı düzgün deyil!";
            }

            return RedirectToAction("Rates");
        }
    }
}