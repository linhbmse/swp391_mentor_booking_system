using Email.Data;
using Email.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Email.Controllers
{
    public class AccountController : Controller
    {
        private readonly SwpFall24Context _Db = new SwpFall24Context();
        private readonly EmailService _emailService;

        public AccountController(IOptions<SmtpSettings> smtpSettings)
        {
            _emailService = new EmailService(smtpSettings);
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
                        // Update first login to false
                      

                        // Send welcome email
                        string subject = "Welcome to FU-NextExam!";
                        string body = $"<h1>Hello,</h1><p>Welcome to Our Mentoring Web! We're excited to have you on board.</p>";
                        await _emailService.SendEmailAsync(user.Email, subject, body);
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
    }
}
