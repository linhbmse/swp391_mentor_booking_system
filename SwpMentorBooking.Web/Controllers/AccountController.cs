using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Web.ViewModels;
using SwpMentorBooking.Application.Common.Interfaces;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private enum Roles
        {
            Admin,
            Student,
            Mentor
        }

        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("RedirectBasedOnRole");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _unitOfWork.User.Get(x => x.Email == model.UserEmail);
                if (user != null && user.Password.Equals(model.Password))
                {
                    if (user.IsActive == false)
                    {
                        return RedirectToAction(nameof(Banned));
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
                            ExpiresUtc = model.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(30),
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), properties);

                        if (user.IsFirstLogin)
                        {
                            ViewData["Password"] = user.Password;
                            return RedirectToAction(nameof(ChangePassword));
                        }
                        return RedirectToAction(nameof(RedirectBasedOnRole));
                    }
                }
                ViewData["ValidateMessage"] = "Username or password is invalid";
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
            if (userRole.Equals("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            if (userRole.Equals("Mentor"))
            {
                return RedirectToAction("Index", "Mentor");
            }
            if (userRole.Equals("Student"))
            {
                return RedirectToAction("Index", "Student");
            }
            return RedirectToAction("Error", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.User.Get(x => x.Email == userEmail);
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
                ViewBag.IsFirstLogin = user?.IsFirstLogin;
                if (user != null)
                {
                    if (user.Password != model.NewPassword )
                    {
                        if (model.CurrentPassword == user.Password)
                        {
                            if (user.IsFirstLogin)
                            {
                                user.IsFirstLogin = false;
                            }
                            user.Password = model.NewPassword;
                            _unitOfWork.Save();
                            return RedirectToAction(nameof(RedirectBasedOnRole));
                        }
                        else
                        {
                            ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("NewPassword", "The new password must be different from the current password.");
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
