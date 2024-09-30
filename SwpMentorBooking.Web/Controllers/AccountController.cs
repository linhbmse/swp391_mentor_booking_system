using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Web.ViewModels;


namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
                var user = _dbContext.Users.Where(x => x.Email == model.UserEmail && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    if (user.IsActive == false)
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
                        return RedirectToAction("RedirectBasedOnRole");
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
            if ("admin".Equals(userRole))
            {
                return RedirectToAction("Index", "Administrator");
            }
            if ("mentor".Equals(userRole))
            {
                return RedirectToAction("Mentor", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userEmail = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == userEmail);
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
                var user = _dbContext.Users.FirstOrDefault(x => x.Email == userEmail);
                if (user != null)
                {
                    if (model.CurrentPassword == user.Password)
                    {
                        if (user.IsFirstLogin)
                        {
                            user.IsFirstLogin = false;
                        }
                        user.Password = model.NewPassword;
                        _dbContext.SaveChanges();
                        return RedirectToAction("Index", "Home");
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

        
    }
}
