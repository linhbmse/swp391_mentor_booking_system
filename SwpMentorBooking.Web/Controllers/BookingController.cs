using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;
using System.Security.Claims;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Student", Policy = "GroupLeaderOnly")]
    [Route("booking")]
    public class BookingController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet("{mentorId}/schedule")]
        public IActionResult BookSchedule(int mentorId, DateTime? startDate)
        {
            MentorDetail mentor = _unitOfWork.Mentor.Get(m => m.UserId == mentorId,
                                        includeProperties: nameof(User));
            if (mentor is null)
            {
                return NotFound();
            }

            DateTime now = startDate ?? DateTime.Now;
            DateOnly currentMonday = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek + 1));
            IEnumerable<Slot> slots = _unitOfWork.Slot.GetAll();
            // Get the selected mentor's schedules
            IEnumerable<MentorSchedule> mentorSchedules = _unitOfWork.MentorSchedule
                .GetAll(ms => ms.MentorDetailId == mentor.UserId && ms.Status != "unavailable", includeProperties: nameof(Slot))
                .OrderBy(ms => ms.Date);

            var mentorScheduleVM = mentorSchedules.Select(s => new MentorScheduleVM
            {
                Id = s.Id,
                MentorDetailId = s.MentorDetailId,
                SlotId = s.SlotId,
                Date = s.Date.ToDateTime(TimeOnly.MinValue),
                Status = s.Status,
                SlotStartTime = s.Slot.StartTime.ToString(@"HH\:mm"),
                SlotEndTime = s.Slot.EndTime.ToString(@"HH\:mm"),

                IsPastSlot = s.Date < DateOnly.FromDateTime(DateTime.Now) ||
                // Or the time is current date and the slot's time has passed
                 (s.Date == DateOnly.FromDateTime(DateTime.Now) &&
                  (s.Slot.StartTime < TimeOnly.FromDateTime(DateTime.Now)))
            }).ToList();

            var mentorScheduleWeekVM = new MentorScheduleWeekVM
            {
                MentorId = mentor.UserId,
                MentorFullName = mentor.User.FullName,
                WeekStartDate = currentMonday.ToDateTime(TimeOnly.MinValue),
                Slots = slots.ToList(),
                DailySchedules = Enumerable.Range(0, 7).Select(i => new DailySchedule
                {
                    Date = currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue),
                    MentorSchedules = mentorScheduleVM
                        .Where(s => s.Date.Date == currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue).Date)
                        .ToList(),
                    IsPastDay = currentMonday.AddDays(i) < DateOnly.FromDateTime(DateTime.Now),
                }).ToList()
            };

            var bookingScheduleVM = new BookingScheduleVM
            {
                MentorSchedule = mentorScheduleWeekVM,
                SelectedSlot = null
            };

            return View(bookingScheduleVM);
        }

        [HttpGet("proceed")]
        public IActionResult ProceedToBooking(int scheduleId)
        {
            var mentorSchedule = _unitOfWork.MentorSchedule.Get(ms => ms.Id == scheduleId,
                includeProperties: $"{nameof(Slot)},MentorDetail.User");
            if (mentorSchedule is null || mentorSchedule.Status != "available")
            {
                return NotFound();
            }

            // Get student detail and check if that student is a leader
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _unitOfWork.Student.Get(s => s.User.Email == userEmail,
                includeProperties: $"{nameof(User)},Group.Topic");
            if (student is null || student.Group is null || !student.IsLeader)
            {
                return NotFound();
            }

            var bookingDetailsVM = new BookingScheduleDetailsVM
            {
                GroupId = student.Group.Id,
                LeaderId = student.UserId,
                MentorScheduleId = scheduleId,
                GroupName = student.Group.GroupName,
                GroupTopic = student.Group.Topic.Name,
                MentorId = mentorSchedule.MentorDetail.UserId,
                MentorName = mentorSchedule.MentorDetail.User.FullName,
                SlotId = mentorSchedule.SlotId,
                ScheduleDate = mentorSchedule.Date.ToDateTime(TimeOnly.MinValue),
                SlotStartTime = mentorSchedule.Slot.StartTime,
                SlotEndTime = mentorSchedule.Slot.EndTime
            };

            return View(bookingDetailsVM);
        }

        [HttpPost("proceed")]
        public IActionResult ProceedToBooking(BookingScheduleDetailsVM bookingDetailsVM)
        {
            // Get the schedule
            var mentorSchedule = _unitOfWork.MentorSchedule.Get(ms => ms.Id == bookingDetailsVM.MentorScheduleId,
                includeProperties: $"{nameof(Slot)},MentorDetail.User");
            if (mentorSchedule is null || mentorSchedule.Status != "available")
            {
                return NotFound();
            }
            // Get the current student 
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _unitOfWork.Student.Get(s => s.User.Email == userEmail,
                includeProperties: $"{nameof(User)},Group.Topic,Group.Wallet");
            if (student is null || student.Group is null || !student.IsLeader)
            {
                return NotFound();
            }
            // Get current wallet balance of the Student's group
            Wallet groupWallet = _unitOfWork.Wallet.Get(w => w.Id == student.Group.WalletId);
            if (groupWallet is null)
            {
                TempData["error"] = "An error has occurred. Please try again.";
                return RedirectToAction(nameof(BookSchedule), mentorSchedule.MentorDetail.UserId);
            }
            // Re-populate the BookingScheduleDetailsVM, this time with the Wallet's balance
            var confirmBookingVM = new BookingScheduleDetailsVM
            {
                GroupId = student.Group.Id,
                LeaderId = student.UserId,
                GroupName = student.Group.GroupName,
                GroupTopic = student.Group.Topic.Name,
                MentorId = mentorSchedule.MentorDetail.UserId,
                MentorName = mentorSchedule.MentorDetail.User.FullName,
                MentorScheduleId = bookingDetailsVM.MentorScheduleId,
                SlotId = mentorSchedule.SlotId,
                ScheduleDate = mentorSchedule.Date.ToDateTime(TimeOnly.MinValue),
                SlotStartTime = mentorSchedule.Slot.StartTime,
                SlotEndTime = mentorSchedule.Slot.EndTime,
                WalletBalance = groupWallet.Balance,
                Note = bookingDetailsVM.Note,
                BookingCost = 10,
            };

            return RedirectToAction(nameof(ConfirmBooking), confirmBookingVM);
        }

        [HttpGet("confirmation")]
        public IActionResult ConfirmBooking(BookingScheduleDetailsVM confirmBookingVM)
        {
            if (confirmBookingVM is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault()?.ErrorMessage;
            }

            return View(confirmBookingVM);
        }

        [HttpPost("finalize")]
        public IActionResult FinalizeBooking(BookingScheduleDetailsVM bookingDetails)
        {
            if (bookingDetails == null)
            {
                return NotFound();
            }

            // TODO: Implement the logic to create the booking in the database
            // For now, we'll just redirect back to the schedule

            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToAction(nameof(BookSchedule), new { mentorId = bookingDetails.MentorId });
        }
    }
}
