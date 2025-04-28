using DEPI_Donation.Data;
using DEPI_Donation.Models;
using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DEPI_Donation.Controllers
{
    public class DonationsController : Controller
    {
        private readonly AppDbcontext _context;
        private readonly DonationBL _donationBL;

        public DonationsController(AppDbcontext context)
        {
            _context = context;
            _donationBL = new DonationBL(context);
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
                _context.Donations.Add(newDonation);

                if(newDonation.Status == DonationStatusType.Confirmed) 
                { 
                var activity = _context.Activities.Find(newDonation.ActivityId);
                if(activity == null) return Json(new { success = false, message = "Invalid Activity Id" });
                activity.CollectedAmount += newDonation.Amount;
                }

                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
                activity.CollectedAmount -= donation.Amount; // deduct the old amount
            if (updatedDonation.Status == DonationStatusType.Confirmed)
                activity.CollectedAmount += updatedDonation.Amount; // add the new amount

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

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var donation = _context.Donations.Find(id);
            if (donation == null)
            {
                return Json(new { success = false, message = "Donation not found." });
            }

            try
            {
                if (donation.Status == DonationStatusType.Confirmed)
                {
                    var activity = _context.Activities.Find(donation.ActivityId);
                    if (activity == null) return Json(new { success = false, message = "Invalid Activity Id" });
                    activity.CollectedAmount -= donation.Amount;
                }
                _context.Donations.Remove(donation);
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
