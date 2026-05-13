using BankSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankDataService _bankService;

        public HomeController(BankDataService bankService)
        {
            _bankService = bankService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            // 1. İstifadəçini və onun blok vəziyyətini yoxla
            var currentUser = _bankService.GetAllUsers().FirstOrDefault(u => u.Id == userId);

            if (currentUser == null || currentUser.IsBlocked)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Hesabınız bloklanıb və ya tapılmadı!";
                return RedirectToAction("Login", "Auth");
            }

            // 2. İstifadəçinin hesablarını gətir
            var userAccounts = _bankService.GetAllAccounts()
                .Where(a => a.UserId == userId)
                .ToList();

            // 3. Tarixçəni (Transactions) gətir və ancaq bu istifadəçiyə aid olanları süz
            // Hesab nömrələrinə görə bu istifadəçinin etdiyi köçürmələri tapırıq
            var userAccNumbers = userAccounts.Select(a => a.AccountNumber).ToList();
            var history = _bankService.GetTransactionHistory()
                .Where(t => userAccNumbers.Contains(t.FromAccountNumber) || userAccNumbers.Contains(t.ToAccountNumber))
                .Take(10) // Son 10 əməliyyat
                .ToList();

            ViewBag.UserName = currentUser.FullName;
            ViewBag.History = history; // Tarixçəni View-ya göndəririk

            return View(userAccounts);
        }

        [HttpPost]
        public IActionResult Transfer(string fromAcc, string toAcc, decimal amount, string targetCurrency)
        {
            if (string.IsNullOrEmpty(fromAcc) || string.IsNullOrEmpty(toAcc) || amount <= 0)
            {
                TempData["Error"] = "Məlumatları düzgün daxil edin.";
                return RedirectToAction("Index");
            }

            bool success = _bankService.MakeTransfer(fromAcc, toAcc, amount, targetCurrency);

            if (!success)
            {
                TempData["Error"] = "Köçürmə baş tutmadı. Balans kifayət deyil və ya hesab nömrəsi yanlışdır.";
            }
            else
            {
                TempData["Success"] = $"Uğurlu əməliyyat! {amount} {targetCurrency} köçürüldü.";
            }

            return RedirectToAction("Index");
        }
    }
}