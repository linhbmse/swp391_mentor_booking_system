using Email.Data;
using Email.Models;
using Email.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Email.Controllers
{
    public class AccountController : Controller
    {
        private readonly SwpFall24Context _Db = new SwpFall24Context();
        private readonly EmailService _emailService;

        public AccountController(IOptions<SmtpSettings> smtpSettings, SwpFall24Context context)
        {
            _emailService = new EmailService(smtpSettings);
            _Db = context;
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }
        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database
                var user = _Db.Users.FirstOrDefault(s => s.Email.Equals(email));

                // Check if the user exists and password matches
                if (user != null && user.Password.Equals(password))
                {
                    // Check if this is the first login
                    if (user.IsFirstLogin) // Assuming you have a property to check first login
                    {
                        Console.WriteLine("First Login here bro ??");



                        // Send welcome email
                        string subject = "Welcome to FU-NextExam!";
                        string body = $"<h1>Hello,</h1><p>Welcome to Our Mentoring Web! We're excited to have you on board.</p>";
                        await _emailService.SendEmailAsync(user.Email, subject, body);
                        // Update first login to false
                        user.IsFirstLogin = false;
                        _Db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                    }

                    // Set session variables (if needed)
                    // Session["FullName"] = user.FirstName + " " + user.LastName;
                    // Session["Email"] = user.Email;
                    // Session["idUser"] = user.idUser;

                    Console.WriteLine("Login Success");
                }
                else
                {
                    ViewBag.error = "Login failed";
                }
            }
            return View();
        }


        // GET: Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Forgot Password (send reset link)
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _Db.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "No user found with that email.");
                return View(model);
            }

            // Build the reset link (without a token)
            var resetLink = Url.Action("ResetPassword", "Account", new { email = user.Email }, Request.Scheme);


            // Send reset link via email
            string subject = "Reset your password";
            string body = $"Please click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(user.Email, subject, body);

            ViewBag.Message = "Password reset link has been sent to your email.";
            return View("ForgotPasswordConfirmation"); // Show a confirmation view
        }

        // GET: Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid request.");
            }

            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }

        // POST: Reset Password (update password in DB)
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _Db.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }

            // Update user's password
            user.Password = model.NewPassword; // You can hash this if required
            _Db.Users.Update(user);
            await _Db.SaveChangesAsync();

            ViewBag.Message = "Your password has been successfully reset.";
            return View("ResetPasswordConfirmation"); // Show confirmation view
        }

    }
}
