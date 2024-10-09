using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Web.ViewModels;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;


namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private enum Roles
        {
            Admin,
            Student,
            Mentor
        }
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                //return RedirectToAction("Index", "Home");
                return RedirectToAction(nameof(RedirectBasedOnRole));
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.User.Get(x => x.Email.Equals(model.UserEmail));
                if (user is not null && user.Password == model.Password)
                {
                    if (!user.IsActive)
                    {
                        return RedirectToAction("Banned", "Account");
                    }
                    else
                    {
                        List<Claim> claims = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                                    new Claim(ClaimTypes.Name, user.FullName),
                                    new Claim(ClaimTypes.Role, user.Role),
                                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = model.RememberMe,
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), properties);

                        if (user.IsFirstLogin)
                        {
                            return RedirectToAction("ChangePassword", "Account");
                        }
                        return RedirectToAction(nameof(RedirectBasedOnRole));
                        //return RedirectToAction("Index", "Home");                
                    }
                }
                ViewData["ValidateMessage"] = "User not found.";
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult RedirectBasedOnRole()
        {
            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if ("Admin".Equals(userRole))
            {
                return RedirectToAction("Index", "Admin");
            }
            if ("Mentor".Equals(userRole))
            {
                return RedirectToAction("Index", "Mentor");
            }
            if (userRole.Equals("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.User.Get(u => u.Email == userEmail);
            ViewBag.IsFirstLogin = user?.IsFirstLogin;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = _unitOfWork.User.Get(u => u.Email == userEmail);
                if (user != null)
                {
                    if (model.CurrentPassword == user.Password)
                    {
                        if (user.IsFirstLogin)
                        {
                            user.IsFirstLogin = false;
                        }
                        user.Password = model.NewPassword;
                        _unitOfWork.Save();
                        return RedirectToAction(nameof(Logout));
                    }
                    else
                    {
                        ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Banned()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
