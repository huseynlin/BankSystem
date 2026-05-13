using Microsoft.AspNetCore.Mvc;
using BankSystem.Services;
using BankSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;

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

        // --- YENİ: İstifadəçinin fərdi tarixçəsini görmək ---
        public IActionResult UserHistory(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Auth");

            var user = _bankService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            // İstifadəçinin hesab nömrələrini götürürük
            var userAccNumbers = user.Accounts.Select(a => a.AccountNumber).ToList();

            // Bütün tarixçə içindən bu istifadəçinin hesabları iştirak edən sətirləri tapırıq
            var history = _bankService.GetTransactionHistory()
                .Where(t => userAccNumbers.Contains(t.FromAccountNumber) || userAccNumbers.Contains(t.ToAccountNumber))
                .ToList();

            ViewBag.TargetUser = user.FullName;
            return View(history); // UserHistory.cshtml səhifəsinə göndəririk
        }

        [HttpGet]
        public IActionResult CreateUser() => View();

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            bool isCreated = _bankService.CreateUser(user);

            if (!isCreated)
            {
                TempData["Error"] = "Bu email artıq istifadə olunub! Zəhmət olmasa başqa email daxil edin.";
                return View(user);
            }

            TempData["Success"] = "İstifadəçi uğurla yaradıldı.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _bankService.GetAllUsers().FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(User user)
        {
            _bankService.UpdateUser(user);
            TempData["Success"] = "Məlumatlar yeniləndi.";
            return RedirectToAction("Index");
        }

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

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            _bankService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ToggleBlock(int id)
        {
            _bankService.ToggleUserBlock(id);
            return RedirectToAction("Index");
        }

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