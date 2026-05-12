using BankSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

            // İstifadəçinin bloklanıb-bloklanmadığını yoxlayaq
            var currentUser = _bankService.GetAllUsers().FirstOrDefault(u => u.Id == userId);
            if (currentUser != null && currentUser.IsBlocked)
            {
                HttpContext.Session.Clear();
                TempData["Error"] = "Hesabınız bloklanıb! Zəhmət olmasa adminlə əlaqə saxlayın.";
                return RedirectToAction("Login", "Auth");
            }

            var allAccounts = _bankService.GetAllAccounts();
            var userAccounts = allAccounts.Where(a => a.UserId == userId).ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View(userAccounts);
        }

        [HttpPost]
        public IActionResult Transfer(string fromAcc, string toAcc, decimal amount, string targetCurrency)
        {
            // Servisdəki yeni MakeTransfer metodunu çağırırıq (4 parametrli)
            bool success = _bankService.MakeTransfer(fromAcc, toAcc, amount, targetCurrency);

            if (!success)
            {
                TempData["Error"] = "Köçürmə baş tutmadı. Balansınızı və ya hesab nömrəsini yoxlayın.";
            }
            else
            {
                TempData["Success"] = $"Uğurlu əməliyyat! {amount} {targetCurrency} göndərildi.";
            }

            return RedirectToAction("Index");
        }
    }
}