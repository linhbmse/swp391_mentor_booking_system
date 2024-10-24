using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;
using System.Security.Claims;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Student, Mentor")]
    [Route("feedback")]
    public class FeedbackController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FeedbackController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("send/{bookingId}")]
        public IActionResult SendFeedback(int bookingId)
        {
            // Retrieve current user's info
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _unitOfWork.User.Get(u => u.Email == userEmail,
                        includeProperties: $"{nameof(StudentDetail)},{nameof(MentorDetail)}");

            if (user is null)
            {
                return NotFound();
            }
            // Retrieve current booking info
            Booking booking = _unitOfWork.Booking.Get(b => b.Id == bookingId && b.Status == "completed",
                includeProperties: "MentorSchedule.MentorDetail.User,Leader.User");

            if (booking is null)
            {
                return NotFound();
            }

            // Check if the user has already given feedback for this booking
            var existingFeedback = _unitOfWork.Feedback.Get(f => f.BookingId == bookingId && f.GivenBy == user.Id);
            if (existingFeedback is not null)
            {
                TempData["error"] = "You have already submitted feedback for this booking.";
                return RedirectToAction("ViewBookingDetail", "Booking", new { bookingId });
            }

            // Populate the View model for display
            var feedbackVM = new FeedbackVM
            {
                BookingId = bookingId,
                GivenBy = user.Id,
                GivenTo = booking.MentorSchedule.MentorDetail.UserId
            };

            return View(feedbackVM);
        }

        [HttpPost("send")]
        public IActionResult SendFeedback(FeedbackVM feedbackVM)
        {
            if (!ModelState.IsValid)
            {
                return View(feedbackVM);
            }

            var feedback = new Feedback
            {
                BookingId = feedbackVM.BookingId,
                GivenBy = feedbackVM.GivenBy,
                GivenTo = feedbackVM.GivenTo,
                Rating = feedbackVM.Rating,
                Comment = feedbackVM.Comment,
                Date = DateTime.Now
            };

            _unitOfWork.Feedback.Add(feedback);
            _unitOfWork.Save();

            TempData["success"] = "Feedback submitted successfully!";
            return RedirectToAction("ViewBookingDetail", nameof(Booking), new { bookingId = feedbackVM.BookingId });
        }

        //[HttpGet("view")]
        //public IActionResult ViewFeedbacks()
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var feedbacks = _unitOfWork.Feedback.GetAll(f => f.GivenTo == userId || f.GivenBy == userId,
        //        includeProperties: "Booking.MentorSchedule.MentorDetail.User,Booking.Leader.User");

        //    return View(feedbacks);
        //}
    }
}
