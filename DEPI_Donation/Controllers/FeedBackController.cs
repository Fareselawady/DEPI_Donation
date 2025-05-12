using DEPI_Donation.Data;
using DEPI_Donation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEPI_Donation.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly AppDbcontext _context;

        public FeedbacksController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: Feedbacks
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var feedbacks = _context.FeedBacks.ToList();
            return View(feedbacks);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(int id)
        {
            try
            {
                var feedback = _context.FeedBacks.FirstOrDefault(f => f.Id == id);
                if (feedback == null)
                {
                    return Json(new { success = false, message = "Feedback not found." });
                }

                _context.FeedBacks.Remove(feedback);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Feedbacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        [HttpPost]
        public JsonResult Create(FeedBack feedback)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed." });
            }

            try
            {
                _context.FeedBacks.Add(feedback);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
