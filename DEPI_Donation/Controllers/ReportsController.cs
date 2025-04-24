using DEPI_Donation.Data;
using DEPI_Donation.Models;
using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbcontext _context;
        private readonly ReportBL _reportBL;

        public ReportsController(AppDbcontext context)
        {
            _context = context;
            _reportBL = new ReportBL(context);
        }

        public IActionResult Index()
        {
            var reports = _context.Reports.Include(r => r.Activities).ToList();
            return View(reports);
        }

        public IActionResult Edit(int id)
        {
            var report = _context.Reports.FirstOrDefault(r => r.ReportId == id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        [HttpPost]
        public JsonResult Edit(int id, Report updatedReport)
        {
            if (id != updatedReport.ReportId)
            {
                return Json(new { success = false, message = "Invalid report ID." });
            }

            var report = _context.Reports.FirstOrDefault(r => r.ReportId == id);
            if (report == null)
            {
                return Json(new { success = false, message = "Report not found." });
            }

            report.Title = updatedReport.Title;
            report.Description = updatedReport.Description;

            try
            {
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Console.WriteLine($"Deleting report with ID: {id}");

                var report = _context.Reports.FirstOrDefault(r => r.ReportId == id);
                if (report == null)
                {
                    Console.WriteLine("Report not found"); // في حالة عدم العثور على المستخدم
                    return Json(new { success = false, message = "Report not found." });
                }

                // حذف أي Activities مرتبطة بالمستخدم
                var activities = _context.Activities.Where(a => a.ReportId == id).ToList();
                if (activities.Any())
                {
                    _context.Activities.RemoveRange(activities);
                }


                _context.Reports.Remove(report);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // GET: Reports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reports/Create
        [HttpPost]
        public JsonResult Create(Report newReport)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            try
            {
                _context.Reports.Add(newReport);
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
