using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/topics")]
    public class TopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Topic> topics = _unitOfWork.Topic.GetAll();

            return View();
        }
    }
}
