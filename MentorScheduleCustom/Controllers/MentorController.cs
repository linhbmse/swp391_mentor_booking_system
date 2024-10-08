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

        // GET: Mentor/ScheduleByWeek
        public IActionResult ScheduleByWeek(int mentorId, DateTime? startDate)
        {
            DateOnly currentMonday = DateOnly.FromDateTime(startDate ?? DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1));

            var schedules = _context.MentorSchedules
                .Include(ms => ms.Slot)
                .Where(ms => ms.MentorDetailId == mentorId && ms.Date >= currentMonday && ms.Date < currentMonday.AddDays(7))
                .OrderBy(ms => ms.Date)
                .ToList();

            var viewModel = new MentorScheduleWeekViewModel
            {
                MentorDetailId = mentorId,
                WeekStartDate = currentMonday.ToDateTime(TimeOnly.MinValue),
                DailySchedules = Enumerable.Range(0, 7).Select(i => new DailySchedule
                {
                    Date = currentMonday.AddDays(i).ToDateTime(TimeOnly.MinValue),
                    MentorSchedules = schedules.Where(s => s.Date == currentMonday.AddDays(i)).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Mentor/ToggleSlotAvailability
        [HttpPost]
        public IActionResult ToggleSlotAvailability(int mentorScheduleId)
        {
            var schedule = _context.MentorSchedules.Find(mentorScheduleId);
            if (schedule == null)
            {
                return NotFound("Schedule not found.");
            }

            // Prevent changes if the slot is already booked
            if (schedule.Status == "booked")
            {
                return BadRequest("This slot is already booked and cannot be changed.");
            }

            // Toggle between "available" and "unavailable"
            schedule.Status = schedule.Status == "available" ? "unavailable" : "available";

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect back to the weekly schedule page
            return RedirectToAction("ScheduleByWeek", new { mentorId = schedule.MentorDetailId });
        }
    }
}
