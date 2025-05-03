using DEPI_Donation.Data;
using DEPI_Donation.Models;
//using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DEPI_Donation.Controllers
{
    public class DonationsController : Controller
    {
        private readonly AppDbcontext _context;
        //private readonly DonationBL _donationBL;

        public DonationsController(AppDbcontext context)
        {
            _context = context;
            //_donationBL = new DonationBL(context);
        }

        public IActionResult Index()
        {
            var donations = _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Payment)
                .Include(d => d.Activity)
                .ToList();
            return View(donations);
        }

        
          public IActionResult Create()
        {
            var activities = _context.Activities.Select(a => new SelectListItem
            {
                Value = a.ActivityId.ToString(),
                Text = a.Title
            }).ToList();

            var payments = _context.Payments.Select(p => new SelectListItem
            {
                Value = p.PaymentId.ToString(),
                Text = p.PaymentMethod
            }).ToList();

            ViewBag.Activities = activities;
            ViewBag.Payments = payments;

            return View();
        }
        

        [HttpPost]
        public JsonResult Create(Donation newDonation)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            try
            {
                newDonation.Status = DonationStatusType.Pending;
                _context.Donations.Add(newDonation);
                _context.SaveChanges();

                // استرجاع البيانات اللازمة للإشعار
                var activity = _context.Activities.FirstOrDefault(a => a.ActivityId == newDonation.ActivityId);
                var donor = _context.Users.FirstOrDefault(u => u.Id == newDonation.DonorId); // Assuming Users table

                if (activity != null && donor != null)
                {
                    // إنشاء الإشعار
                    var notification = new Notification
                    {
                        Title = "New Donation",
                        Description = $"You Donated To '{activity.Title}' With Amount '{newDonation.Amount}' At '{newDonation.DonationDate?.ToString("yyyy-MM-dd")}'",
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(notification);
                    _context.SaveChanges();

                    // ربط الإشعار بالمتبرع (DonorNotification)
                    var donorNotification = new DonorNotification
                    {
                        DonorId = donor.Id,
                        NotificationId = notification.NotificationId
                    };
                    _context.DonorNotifications.Add(donorNotification);
                    _context.SaveChanges();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Cancel(int id)
        {
            var donation = _context.Donations.FirstOrDefault(d => d.DonationId == id);
            if (donation == null)
            {
                return Json(new { success = false, message = "Donation not found." });
            }

            if (donation.Status != DonationStatusType.Pending)
            {
                return Json(new { success = false, message = "Only pending donations can be canceled." });
            }

            donation.Status = DonationStatusType.Canceled;

            _context.SaveChanges(); 

            return Json(new { success = true });
        }

        public IActionResult Edit(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            return View(donation);
        }


        [HttpPost]
        public JsonResult Approve(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation == null)
                return Json(new { success = false, message = "Donation not found." });

            if (donation.Status != DonationStatusType.Pending)
                return Json(new { success = false, message = "Donation is not pending." });

            var activity = _context.Activities.Find(donation.ActivityId);
            if (activity == null)
                return Json(new { success = false, message = "Activity not found." });

            activity.CollectedAmount += donation.Amount;

            if (activity.CollectedAmount >= activity.TargetAmount)
            {
                activity.Status = "Completed"; 
            }

            donation.Status = DonationStatusType.Confirmed;

            _context.SaveChanges(); 

            return Json(new { success = true });
        }



        [HttpPost]
        public JsonResult Edit(int id, Donation updatedDonation)
        {
            if (id != updatedDonation.DonationId)
            {
                return Json(new { success = false, message = "Invalid donation ID." });
            }

            var donation = _context.Donations.Find(id);
            if (donation == null)
            {
                return Json(new { success = false, message = "Donation not found." });
            }

            var activity = _context.Activities.Find(donation.ActivityId);
            if (activity == null) return Json(new { success = false, message = "Invalid Activity Id" });
            if (donation.Status == DonationStatusType.Confirmed)
                activity.CollectedAmount -= donation.Amount; 
            if (updatedDonation.Status == DonationStatusType.Confirmed)
                activity.CollectedAmount += updatedDonation.Amount; 

            donation.DonorId = updatedDonation.DonorId;
            donation.PaymentId = updatedDonation.PaymentId;
            donation.ActivityId = updatedDonation.ActivityId;
            donation.Amount = updatedDonation.Amount;
            donation.DonationDate = updatedDonation.DonationDate;
            donation.Status = updatedDonation.Status;

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

        
        
    }
}
