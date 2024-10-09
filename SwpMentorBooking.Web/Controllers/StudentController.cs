using Demo.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;
using System.Security.Claims;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public StudentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyProfile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: nameof(StudentDetail));

            if (user is null)
            {
                return NotFound();
            }
            // Ensure the user is only accessing their own profile
            if (user.Role != "Student")
            {
                return Forbid();
            }

            var studentProfileVM = new StudentDetailVM
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.Phone,
                Gender = user.Gender,
                StudentCode = user.StudentDetail?.StudentCode
            };

            return View(studentProfileVM);
        }

        public IActionResult MyGroup()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _unitOfWork.User.Get(u => u.Email == userEmail, includeProperties: nameof(StudentDetail));

            if (user is null)
            {
                return NotFound();
            }

            StudentDetail studentDetail = user.StudentDetail;
            // Get the Student group info
            StudentGroup studentGroup = _unitOfWork.StudentGroup.Get(g => g.Id == studentDetail.GroupId,
                        includeProperties: $"{nameof(Topic)},{nameof(Wallet)},StudentDetails.User");
            List<StudentDetail> groupMembers = studentGroup.StudentDetails.ToList();

            StudentGroupDetailVM studentGroupDetailVM = new StudentGroupDetailVM
            {
                Student = studentDetail,
                StudentGroup = studentGroup,
                GroupMembers = groupMembers
            };

            return View(studentGroupDetailVM);
        }
    }
}
