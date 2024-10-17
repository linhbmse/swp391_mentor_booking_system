using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;

namespace SwpMentorBooking.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("admin/skill-and-spec")]
    public class SkillSpecializationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkillSpecializationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            IEnumerable<Skill> skills = _unitOfWork.Skill.GetAll();
            IEnumerable<Specialization> specs = _unitOfWork.Specialization.GetAll();

            ManageSkillAndSpecsVM skillAndSpecsVM = new ManageSkillAndSpecsVM
            {
                Skills = skills.ToList(),
                Specs = specs.ToList(),
                NewSkill = new Skill(),
                NewSpec = new Specialization()
            };
            return View(skillAndSpecsVM);
        }

        [HttpPost("add-skill")]
        public IActionResult AddSkill(ManageSkillAndSpecsVM model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Skill.Add(model.NewSkill);
                _unitOfWork.Save();
                TempData["success"] = "Skill added successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), model);
        }

        [HttpGet("edit-skill/{id}")]
        public IActionResult EditSkill(int id)
        {
            var skill = _unitOfWork.Skill.Get(s => s.Id == id);

            if (skill is null)
            {
                return NotFound();
            }
            return View(skill);
        }

        [HttpPost("edit-skill/{id}")]
        public IActionResult EditSkill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Skill.Update(skill);
                _unitOfWork.Save();
                TempData["success"] = "Skill updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        [HttpGet("delete-skill/{id}")]
        public IActionResult DeleteSkill(int id)
        {
            var skill = _unitOfWork.Skill.Get(s => s.Id == id);
            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        [HttpPost("delete-skill/{id}")]
        public IActionResult DeleteSkill(Skill skill)
        {
            var skillToDelete = _unitOfWork.Skill.Get(s => s.Id == skill.Id);

            if (skillToDelete is null)
            {
                TempData["error"] = "An error occurred. Please try again.";
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.Skill.Delete(skillToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Skill deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("add-spec")]
        public IActionResult AddSpec(ManageSkillAndSpecsVM model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Specialization.Add(model.NewSpec);
                _unitOfWork.Save();
                TempData["success"] = "Specialization added successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), model);
        }

        [HttpGet("edit-spec/{id}")]
        public IActionResult EditSpec(int id)
        {
            var spec = _unitOfWork.Specialization.Get(s => s.Id == id);
            if (spec == null)
            {
                return NotFound();
            }
            return View(spec);
        }

        [HttpPost("edit-spec/{id}")]
        public IActionResult EditSpec(Specialization spec)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Specialization.Update(spec);
                _unitOfWork.Save();
                TempData["success"] = "Specialization updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(spec);
        }

        [HttpGet("delete-spec/{id}")]
        public IActionResult DeleteSpec(int id)
        {
            var spec = _unitOfWork.Specialization.Get(s => s.Id == id);
            if (spec is null)
            {
                return NotFound();
            }

            return View(spec);
        }

        [HttpPost("delete-spec/{id}")]
        public IActionResult DeleteSpec(Specialization spec)
        {
            var specToDelete = _unitOfWork.Specialization.Get(s => s.Id == spec.Id);
            if (specToDelete is null)
            {
                TempData["error"] = "An error occurred. Please try again.";
                return RedirectToAction(nameof(Index));
            }

            _unitOfWork.Specialization.Delete(specToDelete);
            _unitOfWork.Save();
            TempData["success"] = "Specialization deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
