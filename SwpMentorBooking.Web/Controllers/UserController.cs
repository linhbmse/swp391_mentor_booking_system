using Microsoft.AspNetCore.Mvc;

namespace SwpMentorBooking.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Import()
        {
            return View();
        }
    }
}
