using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcoSanitizeApp.Data;
using EcoSanitizeApp.Models;

namespace EcoSanitizeApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var input = model.Username.Trim().ToLower();

            // Support authentication via either username or email
            var user = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == input || u.Email.ToLower() == input);

            if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username/email or password. Please check your credentials.");
                return View(model);
            }

            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("FullName", user.FullName);
            HttpContext.Session.SetString("UserRole", user.Role);

            TempData["SuccessMessage"] = $"Welcome back, {user.FullName} ({user.Role}).";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Enforce single administrator policy: No public registration as admin
            if (string.Equals(model.Role, "System Administrator", StringComparison.OrdinalIgnoreCase) ||
                model.Role.Contains("Admin", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Role", "System Administrator accounts cannot be created via public registration. Only one dedicated System Administrator exists in the system.");
                return View(model);
            }

            bool adminExists = _context.Users.Any(u => u.Role == "System Administrator");
            if (adminExists && model.Role.Contains("Admin", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Role", "A System Administrator already exists. Additional administrator accounts are restricted.");
                return View(model);
            }

            // Check if username already taken
            if (_context.Users.Any(u => u.Username.ToLower() == model.Username.Trim().ToLower()))
            {
                ModelState.AddModelError("Username", "This username is already taken. Please choose another username.");
                return View(model);
            }

            // Check if email already registered
            if (_context.Users.Any(u => u.Email.ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email", "An account with this email address already exists. Please sign in instead.");
                return View(model);
            }

            var newUser = new AppUser
            {
                FullName = model.FullName.Trim(),
                Username = model.Username.Trim(),
                Email = model.Email.Trim().ToLower(),
                PasswordHash = HashPassword(model.Password),
                Role = model.Role,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetString("Username", newUser.Username);
            HttpContext.Session.SetString("FullName", newUser.FullName);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            TempData["SuccessMessage"] = $"Account created successfully. Welcome, {newUser.FullName}!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out of the session.";
            return RedirectToAction("Index", "Home");
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;
            // Plaintext comparison for default seeded demo accounts (officer123, admin123, citizen123)
            if (storedHash == inputPassword) return true;
            // SHA-256 hash comparison for securely registered users
            return storedHash == HashPassword(inputPassword);
        }
    }
}
