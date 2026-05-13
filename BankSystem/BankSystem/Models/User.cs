using System;
using System.Collections.Generic;

namespace BankSystem.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
        public bool IsBlocked { get; set; } = false;

        // YENİ: Qeydiyyat tarixi
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // SQL əlaqəsi üçün: Bir istifadəçinin çoxlu hesabı ola bilər
        public List<Account> Accounts { get; set; } = new();
    }
}