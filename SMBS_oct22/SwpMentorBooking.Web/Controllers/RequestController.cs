using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Application.Common.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SwpMentorBooking.Infrastructure.Data;
using System.Security.Claims;
using SwpMentorBooking.Web.ViewModels;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Student")] // Only students can access this controller
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public RequestController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Display list of requests for the logged-in leader
        public IActionResult Index()
        {
            var leaderId = GetUserId(); // Fetch the leader's ID
            var requests = _context.Requests
                                   .Where(r => r.LeaderId == leaderId)
                                   .ToList();

            return View("~/Views/Student/RequestPage.cshtml", requests); // Passing requests to view directly
        }

        [Authorize(Policy = "GroupLeaderOnly")] // Only group leaders can send requests
        [HttpGet("student/request/send")]  // Unique route for the GET request
        public IActionResult SendRequest()
        {
            return View("~/Views/Student/SendRequest.cshtml", new RequestViewModel());
        }

        // POST: Handle submission of a new request
        [Authorize(Policy = "GroupLeaderOnly")] // Only group leaders can send requests
        [HttpPost("student/request/send")]  // Unique route for the POST request
        public async Task<IActionResult> SendRequest(RequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var leaderId = GetUserId(); // Fetch the leader's ID

                var newRequest = new Request
                {
                    Title = model.Title,
                    Content = model.Content,
                    LeaderId = leaderId,
                    Timestamp = DateTime.Now,
                    Status = "pending"
                };

                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                // Send email notification to Admin
                await _emailService.SendEmail("maihainam8@gmail.com",
                    "New Request Submitted",
                    $"A new request has been submitted by Leader ID {leaderId} with the title '{model.Title}' at {DateTime.Now}.");

                return RedirectToAction("Index");
            }

            return View("~/Views/Student/SendRequest.cshtml", model);
        }

        // GET: View details of a specific request
        public IActionResult Details(int id)
        {
            var request = _context.Requests.FirstOrDefault(r => r.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View("~/Views/Student/Details.cshtml", request); // Ensure that the Details.cshtml view exists
        }

        // Helper method to get the user's ID from claims
        private int GetUserId()
        {
            // Retrieve the email from the NameIdentifier claim
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier); // This is how email is stored during login

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("Email not found in claims.");
            }

            // Convert both sides to lower case for case-insensitive comparison
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == userEmail.ToLower());

            if (user == null)
            {
                throw new Exception($"User not found for email: {userEmail}");
            }

            return user.Id;
        }



    }
}
