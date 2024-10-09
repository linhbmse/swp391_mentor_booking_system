using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;
using System.Security.Claims;

namespace SwpMentorBooking.Web.Controllers
{
    public class MentorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUtilService _utilService;
        public MentorController(IUnitOfWork unitOfWork, IUtilService utilService)
        {
            _unitOfWork = unitOfWork;
            _utilService = utilService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Mentor")]
        public IActionResult MyProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: nameof(MentorDetail));

            if (user is null)
            {
                return NotFound();
            }
            // Ensure the user is only accessing their own profile
            if (user.Role != "Mentor")
            {
                return Forbid();
            }
            // At this point, the user does exist and it's our job to display the data correctly.

            var mentorProfileVM = new MentorDetailVM
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Gender = user.Gender,
                // Split the properties by the delimiter
                MainProgrammingLanguage = _utilService.StringManipulation.SplitProperty(user.MentorDetail?.MainProgrammingLanguage, "||"),
                AltProgrammingLanguage = _utilService.StringManipulation.SplitProperty(user.MentorDetail?.AltProgrammingLanguage, "||"),
                Framework = _utilService.StringManipulation.SplitProperty(user.MentorDetail?.Framework, "||"),
                Education = _utilService.StringManipulation.SplitProperty(user.MentorDetail?.Education, "||"),
                AdditionalContactInfo = _utilService.StringManipulation.SplitProperty(user.MentorDetail?.AdditionalContactInfo, "||"),
                BookingScore = user.MentorDetail?.BookingScore,
                Description = user.MentorDetail?.Description,
            };

            return View(mentorProfileVM);
        }
    }
}
