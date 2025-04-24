using DEPI_Donation.Data;
using DEPI_Donation.Models;
using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var reports = _context.Reports.Include(r => r.Activity).ToList();
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
                    Console.WriteLine("Report not found");
                    return Json(new { success = false, message = "Report not found." });
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
            var activities = _context.Activities
                                     .Select(a => new SelectListItem
                                     {
                                         Value = a.ActivityId.ToString(),
                                         Text = a.Title
                                     }).ToList();

            ViewBag.Activities = activities;

            return View();
        }



        // POST: Reports/Create
        [HttpPost]
        public JsonResult Create(Report report, int activityId)
        {
            try
            {
                // العثور على النشاط المرتبط بالتقرير
                var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == activityId);

                if (activity != null)
                {
                    report.ActivityId = activity.ActivityId;  // تعيين الـ ActivityId للتقرير
                    _context.Reports.Add(report); // إضافة التقرير
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Activity not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }





    }
}
