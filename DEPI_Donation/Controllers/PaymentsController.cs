using DEPI_Donation.Data;
using DEPI_Donation.Models;
//using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly AppDbcontext _context;
        //private readonly PaymentBL _paymentBL;

        public PaymentsController(AppDbcontext context)
        {
            _context = context;
            //_paymentBL = new PaymentBL(context);

        }

        // GET: Payments
        public IActionResult Index()
        {
            var payments = _context.Payments.Include(p => p.Donations).ToList();
            return View(payments);
        }

        // GET: Payments/Edit/5
        public IActionResult Edit(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        [HttpPost]
        public JsonResult Edit(int id, Payment updatedPayment)
        {
            if (id != updatedPayment.PaymentId)
            {
                return Json(new { success = false, message = "Invalid payment ID." });
            }

            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return Json(new { success = false, message = "Payment not found." });
            }

            payment.PaymentMethod = updatedPayment.PaymentMethod;

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

        // POST: Payments/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
                if (payment == null)
                {
                    return Json(new { success = false, message = "Payment not found." });
                }

                _context.Payments.Remove(payment);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        [HttpPost]
        public JsonResult Create(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
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
