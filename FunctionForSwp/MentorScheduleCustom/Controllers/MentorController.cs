using MentorScheduleCustom.Data;
using MentorScheduleCustom.Models;
using MentorScheduleCustom.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MentorScheduleCustom.Controllers
{
    public class MentorController : Controller
    {
        private readonly SwpFall24Context _context;

        public MentorController(SwpFall24Context context)
        {
            _context = context;
        }

        // Action for viewing booked and available slots
        public IActionResult Schedule(int mentorId, DateTime? startDate)
        {
            DateTime now = startDate ?? DateTime.Now;
            DateOnly currentMonday = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek + 1)); // Ensure Monday is first day
            var schedules = _context.MentorSchedules
                .Include(ms => ms.Slot)
                .Where(ms => ms.MentorDetailId == mentorId && ms.Status != "unavailable")
                .OrderBy(ms => ms.Date)
                .ToList();

            // Map from MentorSchedule to MentorScheduleViewModel
            var scheduleViewModels = schedules.Select(s => new MentorScheduleViewModel
            {
                Id = s.Id,
                MentorDetailId = s.MentorDetailId,
                SlotId = s.SlotId,
                Date = s.Date.ToDateTime(TimeOnly.MinValue),
                Status = s.Status,
                SlotStartTime = s.Slot.StartTime.ToString(@"hh\:mm"),
                SlotEndTime = s.Slot.EndTime.ToString(@"hh\:mm")
            }).ToList();

            var viewModel = new MentorScheduleWeekViewModel
            {
                MentorDetailId = mentorId,
                WeekStartDate = currentMonday.ToDateTime(TimeOnly.MinValue),
                DailySchedules = Enumerable.Range(0, 7).Select(i => new DailySchedule
                {
                    Date = currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue),
                    MentorSchedules = scheduleViewModels
                        .Where(s => s.Date.Date == currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue).Date)
                        .ToList()
                }).ToList()
            };

            return View(viewModel);
        }


        // Action for setting the schedule (all slots are shown, including unavailable)
        public IActionResult SetSchedule(int mentorId, DateTime? startDate)
        {
            // Make sure we always calculate Monday of the current week
            DateTime now = startDate ?? DateTime.Now;
            DateOnly currentMonday = DateOnly.FromDateTime(now.AddDays(-(int)now.DayOfWeek + 1)); // Ensure Monday is first day
            var schedules = _context.MentorSchedules
                .Include(ms => ms.Slot)  // Include related data like Slot if necessary
                .Where(ms => ms.MentorDetailId == mentorId)
                .OrderBy(ms => ms.Date)
                .ToList();

            // Map from Models.MentorSchedule to ViewModel.MentorScheduleViewModel
            var scheduleViewModels = schedules.Select(s => new MentorScheduleViewModel
            {
                Id = s.Id,
                MentorDetailId = s.MentorDetailId,
                SlotId = s.SlotId,
                Date = s.Date.ToDateTime(TimeOnly.MinValue),
                Status = s.Status,
                SlotStartTime = s.Slot.StartTime.ToString(@"hh\:mm"),
                SlotEndTime = s.Slot.EndTime.ToString(@"hh\:mm")
            }).ToList();

            var viewModel = new MentorScheduleWeekViewModel
            {
                MentorDetailId = mentorId,
                WeekStartDate = currentMonday.ToDateTime(TimeOnly.MinValue),
                DailySchedules = Enumerable.Range(0, 7).Select(i => new DailySchedule
                {
                    Date = currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue),
                    MentorSchedules = scheduleViewModels
                        .Where(s => s.Date.Date == currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue).Date)
                        .ToList()
                }).ToList()
            };

            return View(viewModel);
        }



        // Method for toggling slot availability
        [HttpPost]
        public IActionResult ToggleSlotAvailability(int mentorScheduleId)
        {
            var schedule = _context.MentorSchedules.Find(mentorScheduleId);
            if (schedule != null && schedule.Status != "booked")
            {
                schedule.Status = schedule.Status == "available" ? "unavailable" : "available";
                _context.SaveChanges();
            }

            return RedirectToAction("SetSchedule", new { mentorId = schedule.MentorDetailId, startDate = schedule.Date });
        }
    }
}
