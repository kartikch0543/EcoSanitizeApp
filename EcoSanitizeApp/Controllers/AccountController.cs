using System;
using System.Linq;
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

            var user = _context.Users.FirstOrDefault(u =>
                u.Username.ToLower() == model.Username.Trim().ToLower() &&
                u.PasswordHash == model.Password);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password. Check demo credentials below.");
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

            bool exists = _context.Users.Any(u =>
                u.Username.ToLower() == model.Username.Trim().ToLower() ||
                u.Email.ToLower() == model.Email.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("Username", "A user with this username or email already exists.");
                return View(model);
            }

            var newUser = new AppUser
            {
                FullName = model.FullName.Trim(),
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                PasswordHash = model.Password,
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
    }
}
