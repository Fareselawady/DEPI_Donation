using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DEPI_Donation.Models;
using DEPI_Donation.Data;

namespace DEPI_Donation.Controllers
{
    public class ActivityController : Controller
    {
        private readonly AppDbcontext _context;

        public ActivityController(AppDbcontext context)
        {
            _context = context;
        }

        // GET: Activity
        public async Task<IActionResult> Index()
        {
            return View(await _context.Activities.ToListAsync());
        }

        // GET: Activity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activity/Create
        [HttpPost]
        public IActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // التأكد من أن TargetAmount يتم تعيينه بشكل صحيح
                    activity.Status = "Incompleted"; // وضع الحالة بشكل افتراضي
                    _context.Activities.Add(activity);
                    _context.SaveChanges();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }

            return Json(new { success = false, message = "Invalid data." });
        }


        // GET: Activity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();

            return View(activity);
        }

        // POST: Activity/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string Title, string Description, string Category, string Status, int target)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
                return Json(new { success = false, message = "Activity not found" });

            activity.Title = Title;
            activity.Description = Description;
            activity.TargetAmount = target;
            activity.Category = Category;
            activity.Status = Status;

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        // GET: Activity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null) return NotFound();

            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return Json(new { success = false, message = "Activity not found." });
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.ActivityId == id);
        }
    }
}
