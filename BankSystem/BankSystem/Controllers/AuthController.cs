using Microsoft.AspNetCore.Mvc;
using BankSystem.Services;
using Microsoft.AspNetCore.Http;

namespace BankSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly BankDataService _bankService;

        public AuthController(BankDataService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToUserPanel();
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _bankService.GetAllUsers()
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // --- YENİ: BLOK YOXLAMASI ---
                if (user.IsBlocked)
                {
                    ViewBag.Error = "Hesabınız bloklanıb! Zəhmət olmasa adminlə əlaqə saxlayın.";
                    return View();
                }

                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole", user.Role);

                return RedirectToUserPanel();
            }

            ViewBag.Error = "Email və ya şifrə yanlışdır!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private IActionResult RedirectToUserPanel()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}