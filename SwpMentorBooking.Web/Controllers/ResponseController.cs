using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;

namespace SwpMentorBooking.Web.Controllers
{
    public class ResponseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResponseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
