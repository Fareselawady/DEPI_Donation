//using DEPI_Donation.Data;
//using DEPI_Donation.Models;
//using Microsoft.EntityFrameworkCore;

//namespace DEPI_Donation.Models.ModelsBL
//{
//    public class PaymentBL
//    {
//        private readonly AppDbcontext _context;

//        public PaymentBL(AppDbcontext context)
//        {
//            _context = context;
//        }

//        public List<Payment> GetAllPayments()
//        {
//            return _context.Payments.Include(p => p.Donations).ToList();
//        }

//        public Payment GetPaymentById(int id)
//        {
//            var payment = _context.Payments
//                                  .Include(p => p.Donations)
//                                  .FirstOrDefault(p => p.PaymentId == id);

//            if (payment == null)
//                throw new Exception("Payment not found.");

//            return payment;
//        }

//        public void AddPayment(Payment newPayment)
//        {
//            _context.Payments.Add(newPayment);
//            _context.SaveChanges();
//        }

//        public void UpdatePayment(int id, Payment updatedPayment)
//        {
//            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
//            if (payment == null)
//                throw new Exception("Payment not found.");

//            payment.PaymentMethod = updatedPayment.PaymentMethod;
//            _context.SaveChanges();
//        }

//        // If you want to re-enable delete logic:
//        // public void DeletePayment(int id)
//        // {
//        //     var payment = _context.Payments
//        //                           .Include(p => p.Donations)
//        //                           .FirstOrDefault(p => p.PaymentId == id);
//        //
//        //     if (payment == null)
//        //         throw new Exception("Payment not found.");
//        //
//        //     // Optional: remove related Donations if needed
//        //     if (payment.Donations.Any())
//        //         _context.Donations.RemoveRange(payment.Donations);
//        //
//        //     _context.Payments.Remove(payment);
//        //     _context.SaveChanges();
//        // }
//    }
//}
