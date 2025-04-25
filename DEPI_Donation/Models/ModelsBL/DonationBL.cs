using DEPI_Donation.Data;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models.ModelsBL
{
    public class DonationBL
    {
        private readonly AppDbcontext _context;

        public DonationBL(AppDbcontext context)
        {
            _context = context;
        }

        public void AddDonation(int? donorId, int? paymentId, int? activityId, decimal? amount, DateOnly? donationDate, DonationStatusType status)
        {
            var donation = new Donation
            {
                DonorId = donorId,
                PaymentId = paymentId,
                ActivityId = activityId,
                Amount = amount,
                DonationDate = donationDate,
                Status = status
            };

            _context.Donations.Add(donation);
            _context.SaveChanges();
        }

        public void UpdateDonation(int donationId, int? donorId, int? paymentId, int? activityId, decimal? amount, DateOnly? donationDate, DonationStatusType status)
        {
            var donation = _context.Donations.FirstOrDefault(d => d.DonationId == donationId);
            if (donation != null)
            {
                donation.DonorId = donorId;
                donation.PaymentId = paymentId;
                donation.ActivityId = activityId;
                donation.Amount = amount;
                donation.DonationDate = donationDate;
                donation.Status = status;

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Donation not found.");
            }
        }

        public void DeleteDonation(int donationId)
        {
            var donation = _context.Donations.FirstOrDefault(d => d.DonationId == donationId);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Donation not found.");
            }
        }

        public Donation GetDonationById(int donationId)
        {
            var donation = _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Payment)
                .Include(d => d.Activity)
                .FirstOrDefault(d => d.DonationId == donationId);

            if (donation != null)
            {
                return donation;
            }
            else
            {
                throw new Exception("Donation not found.");
            }
        }

        public List<Donation> GetAllDonations()
        {
            return _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Payment)
                .Include(d => d.Activity)
                .ToList();
        }

        public List<Donation> GetDonationsByDonorId(int donorId)
        {
            return _context.Donations
                .Where(d => d.DonorId == donorId)
                .Include(d => d.Payment)
                .Include(d => d.Activity)
                .ToList();
        }

        public List<Donation> GetDonationsByActivityId(int activityId)
        {
            return _context.Donations
                .Where(d => d.ActivityId == activityId)
                .Include(d => d.Donor)
                .Include(d => d.Payment)
                .ToList();
        }
    }
}
