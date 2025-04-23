using DEPI_Donation.Data;
using DEPI_Donation.Models;
using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbcontext _context;
        private readonly UserBL _userBL;

        public UsersController(AppDbcontext context)
        {
            _context = context;
            _userBL = new UserBL(context);
        }

        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Donations).ToList();
            return View(users);
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public JsonResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return Json(new { success = false, message = "Invalid user ID." });
            }
            if (string.IsNullOrEmpty(updatedUser.UserType))
            {
                updatedUser.UserType = "Donor";
            }
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            if (updatedUser.UserType != "Admin" && updatedUser.UserType != "Donor")
            {
                return Json(new { success = false, message = "UserType must be either 'Admin' or 'Donor'." });
            }
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Password = updatedUser.Password;
            user.UserType = updatedUser.UserType;

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
                Console.WriteLine($"Deleting user with ID: {id}"); 

                var user = _context.Users.FirstOrDefault(u => u.UserId == id);
                if (user == null)
                {
                    Console.WriteLine("User not found"); // في حالة عدم العثور على المستخدم
                    return Json(new { success = false, message = "User not found." });
                }

                // حذف أي Notifications مرتبطة بالمستخدم
                var notifications = _context.DonorNotifications.Where(n => n.DonorId == id).ToList();
                if (notifications.Any())
                {
                    _context.DonorNotifications.RemoveRange(notifications);
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public JsonResult Create(User newUser)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            try
            {
                newUser.CreatedAt = DateTime.Now;
                _context.Users.Add(newUser);
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
