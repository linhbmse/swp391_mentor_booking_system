using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Web.ViewModels;
using System.Linq;
using SwpMentorBooking.Application.Common.Interfaces;

namespace SwpMentorBooking.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SearchByName(string searchTemp)
        {
            var mentors = _unitOfWork.Mentor.GetAll(includeProperties: nameof(User));

            if (!string.IsNullOrWhiteSpace(searchTemp))
            {
                mentors = mentors.Where(m =>
                    m.User.FullName.Contains(searchTemp, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(mentors); 
        }

        public IActionResult Search(SearchMentorVM model)
        {
            var mentors = _unitOfWork.Mentor.GetAll(includeProperties: nameof(User));
            
            // Filter by BookingScore range if specified
            if (model.minBookingScore.HasValue || model.maxBookingScore.HasValue)
            {
                mentors = mentors.Where(m => 
                    (!model.minBookingScore.HasValue || m.BookingScore >= model.minBookingScore) &&
                    (!model.maxBookingScore.HasValue || m.BookingScore <= model.maxBookingScore)).ToList();
            }

            // Filter by ProgrammingLanguages if specified
            if (model.ProgrammingLanguage != null && model.ProgrammingLanguage.Any())
            {
                mentors = mentors.Where(m => 
                    model.ProgrammingLanguage.Contains(m.MainProgrammingLanguage)).ToList();
            }

            // Filter by Name if specified
            if (!string.IsNullOrWhiteSpace(model.Name))
            {
                mentors = mentors.Where(m => 
                    m.User.FullName.Contains(model.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(mentors); // Return the filtered list to the view
        }
    }
}
