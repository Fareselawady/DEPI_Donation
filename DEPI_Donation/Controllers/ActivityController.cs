using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DEPI_Donation.Models;
using DEPI_Donation.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Globalization;

namespace DEPI_Donation.Controllers
{
    public class ActivityController : Controller
    {
        private readonly AppDbcontext _context;

        public ActivityController(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.PaymentMethods = await _context.Payments.ToListAsync();

            return View(await _context.Activities.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Activity activity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    activity.Status = "Incompleted";
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();

            return View(activity);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var activity = await _context.Activities
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (activity == null) return NotFound();

            return View(activity);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        public IActionResult GetPaymentMethods()
        {
            var paymentMethods = _context.Payments.ToList(); // تأكد من أن PaymentMethods هو جدول طرق الدفع في قاعدة البيانات
            return Json(paymentMethods); // العودة بالبيانات بتنسيق JSON
        }

        // تبرع
        [HttpPost]
        [Authorize]
      
        public async Task<IActionResult> Donate(int activityId, int paymentMethod, decimal amount)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdString, out int userId))
                {
                    return Json(new { success = false, message = "User ID is invalid or missing." });
                }

                var activity = await _context.Activities.FindAsync(activityId);
                var payment = await _context.Payments.FindAsync(paymentMethod);

                if (activity == null || payment == null)
                {
                    return Json(new { success = false, message = "Invalid activity or payment method." });
                }

                var donation = new Donation
                {
                    ActivityId = activityId,
                    PaymentId = paymentMethod,
                    Amount = amount,
                    DonationDate = DateOnly.FromDateTime(DateTime.Now),
                    DonorId = userId
                };

                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


    }
}
