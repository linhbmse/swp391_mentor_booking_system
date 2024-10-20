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
        private const int _bookingCost = 10;
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
        // Finalize booking => Create a new Booking to the database
        public IActionResult FinalizeBooking(BookingScheduleDetailsVM bookingDetailsVM)
        {
            if (bookingDetailsVM is null || !ModelState.IsValid)
            {
                TempData["error"] = "An error has occurred processing booking details. Please try again.";
                return RedirectToAction(nameof(ConfirmBooking), bookingDetailsVM);
            }
            // Double-check the wallet balance
            if (bookingDetailsVM.BalanceAfter <= 0)
            {
                TempData["ErrorMessage"] = "Insufficient balance to proceed with booking.";
                return RedirectToAction(nameof(ConfirmBooking), bookingDetailsVM);
            }
            // Retrieve necessary information for updates
            Wallet wallet = _unitOfWork.Wallet.Get(w => w.StudentGroup.Id == bookingDetailsVM.GroupId);
            MentorSchedule mentorSchedule = _unitOfWork.MentorSchedule.Get(ms => ms.Id == bookingDetailsVM.MentorScheduleId);

            if (wallet is null || mentorSchedule is null)
            {   // Display error
                TempData["error"] = "An error has occurred processing booking details. Please try again.";
                return RedirectToAction(nameof(BookSchedule), bookingDetailsVM.MentorId);
            }
            // Implement the logic to create the booking in the database
            Booking booking = new Booking
            {
                LeaderId = bookingDetailsVM.LeaderId,
                MentorScheduleId = bookingDetailsVM.MentorScheduleId,
                Timestamp = DateTime.Now,
                Note = bookingDetailsVM.Note,
                Status = "pending"
            };
            // Initiate transaction
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Booking.Add(booking);

                    // Update the wallet balance
                    wallet.Balance = (wallet.Balance - _bookingCost);
                    _unitOfWork.Wallet.Update(wallet);

                    // Update the mentor schedule status
                    mentorSchedule.Status = "booked";
                    _unitOfWork.MentorSchedule.Update(mentorSchedule);
                    // Save changes
                    _unitOfWork.Save();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["error"] = "An error has occurred processing booking details. Please try again.";
                    return RedirectToAction(nameof(BookSchedule), bookingDetailsVM.MentorId);
                }
            }
            TempData["SuccessMessage"] = "Booking confirmed successfully!";
            return RedirectToAction(nameof(BookingSuccess), new { bookingId = booking.Id });
        }

        [HttpGet("success")]
        public IActionResult BookingSuccess(int bookingId)
        {
            // Retrieve the booked schedule info
            Booking booking = _unitOfWork.Booking.Get(b => b.Id == bookingId,
                includeProperties: "MentorSchedule.MentorDetail.User,MentorSchedule.Slot,Leader.User,Leader.Group");

            if (booking is null)
            {
                return NotFound();
            }

            var bookingSuccessVM = new BookingSuccessVM
            {
                BookingId = booking.Id,
                GroupName = booking.Leader.Group.GroupName,
                MentorName = booking.MentorSchedule.MentorDetail.User.FullName,
                ScheduleDate = booking.MentorSchedule.Date.ToDateTime(TimeOnly.MinValue),
                SlotId = booking.MentorSchedule.SlotId,
                SlotStartTime = booking.MentorSchedule.Slot.StartTime,
                SlotEndTime = booking.MentorSchedule.Slot.EndTime,
                Note = booking.Note,
                BookingCost = _bookingCost,
                Timestamp = booking.Timestamp,

                GroupId = booking.Leader.GroupId,
                MentorId = booking.MentorSchedule.MentorDetail.UserId
            };

            return View(bookingSuccessVM);
        }

        [HttpGet("view-all")]
        [AllowAnonymous]
        public IActionResult ViewBookings()
        {
            // Get Student and their group info

            // The reason is we have to consider the case where a Student that does not belong to any group
            // wants to view the bookings. We redirect that Student to the corresponding View.
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _unitOfWork.Student.Get(u => u.User.Email == userEmail, includeProperties: nameof(User));

            if (student is null)
            {
                return NotFound();
            }
            // Get the Student group info
            StudentGroup studentGroup = _unitOfWork.StudentGroup.Get(g => g.Id == student.GroupId,
                        includeProperties: $"StudentDetails.User");

            if (studentGroup is null)
            {
                return View();
            }

            IEnumerable<Booking> bookings = _unitOfWork.Booking.GetAll(b => b.Leader.GroupId == studentGroup.Id,
        includeProperties: "MentorSchedule.MentorDetail.User,MentorSchedule.Slot,Leader.User,Leader.Group"
        ).OrderByDescending(b => b.Timestamp);

            IEnumerable<ViewBookingVM> viewBookingsVM = bookings.Select(b => new ViewBookingVM
            {
                BookingId = b.Id,
                GroupName = b.Leader.Group.GroupName,
                MentorName = b.MentorSchedule.MentorDetail.User.FullName,
                ScheduleDate = b.MentorSchedule.Date.ToDateTime(TimeOnly.MinValue),
                SlotStartTime = b.MentorSchedule.Slot.StartTime,
                SlotEndTime = b.MentorSchedule.Slot.EndTime,
                Note = b.Note,
                BookingCost = _bookingCost,
                Timestamp = b.Timestamp,
                Status = b.Status
            }).ToList();

            return View(viewBookingsVM);
        }
    }
}
