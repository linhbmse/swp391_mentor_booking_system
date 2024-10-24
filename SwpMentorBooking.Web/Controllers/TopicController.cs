using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Web.ViewModels;

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

        [HttpGet("all")]
        public IActionResult Index()
        {
            IEnumerable<Topic> topics = _unitOfWork.Topic.GetAll();

            ManageTopicVM manageTopicVM = new ManageTopicVM
            {
                Topics = topics.ToList(),
                NewTopic = new Topic(),
            };

            return View(manageTopicVM);
        }

        [HttpPost("add-topic")]
        public IActionResult AddTopic(ManageTopicVM manageTopicVM)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Index), manageTopicVM);
            }
            // Model is valid
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Topic.Add(manageTopicVM.NewTopic);
                    _unitOfWork.Save();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["error"] = "An error has occurred when adding new topic. Please try again.";
                    return View(nameof(Index), manageTopicVM);
                }
            }
            // Topic creation is successful
            TempData["success"] = "Topic added successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("edit-topic")]
        public IActionResult EditTopic(int topicId)
        {
            Topic topicToUpdate = _unitOfWork.Topic.Get(t => t.Id == topicId);

            if (topicToUpdate is null)
            {
                TempData["error"] = "An error has occurred. Please try again.";
                return View(nameof(Index));
            }
            // Topic is existent in database
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Topic.Update(topicToUpdate);
                    _unitOfWork.Save();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["error"] = "An error has occurred when updating the topic. Please try again.";
                    return View(nameof(Index));
                }
            }
            // Topic update is successful
            TempData["success"] = "Topic has been updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("delete-topic")]
        public IActionResult DeleteTopic(int topicId)
        {
            Topic topicToUpdate = _unitOfWork.Topic.Get(t => t.Id == topicId);

            if (topicToUpdate is null)
            {
                TempData["error"] = "An error has occurred. Please try again.";
                return View(nameof(Index));
            }
            // Topic is existent in database
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    _unitOfWork.Topic.Delete(topicToUpdate);
                    _unitOfWork.Save();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["error"] = "An error has occurred when deleting the topic. Please try again.";
                    return View(nameof(Index));
                }
            }
            // Topic update is successful
            TempData["success"] = "Topic has been deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}