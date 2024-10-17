using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
                SlotEndTime = s.Slot.EndTime.ToString(@"HH\:mm")
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
                        .ToList()
                }).ToList()
            };

            var bookingScheduleVM = new BookingScheduleVM
            {
                MentorSchedule = mentorScheduleWeekVM,
                SelectedSlot = null
            };

            return View(bookingScheduleVM);
        }
    }
}
