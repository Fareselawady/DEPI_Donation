using DEPI_Donation.Data;
using DEPI_Donation.Models;
using Microsoft.AspNetCore.Authorization;

//using DEPI_Donation.Models.ModelsBL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbcontext _context;
        //private readonly UserBL _userBL;
        private readonly UserManager<User> _userManager;
        public UsersController(AppDbcontext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
            //_userBL = new UserBL(context);
        }

        public IActionResult Index()
        {
            var users = _context.Users.Include(u => u.Donations).ToList();
            return View(users);
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return Json(new { success = false, message = "Invalid user ID." });
            }
            //if (string.IsNullOrEmpty(updatedUser.UserType))
            //{
            //    updatedUser.UserType = "Donor";
            //}
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }
            //if (updatedUser.UserType != "Admin" && updatedUser.UserType != "Donor")
            //{
            //    return Json(new { success = false, message = "UserType must be either 'Admin' or 'Donor'." });
            //}
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.PhoneNumber  = updatedUser.PhoneNumber;
            //user.Password = updatedUser.Password;
            //user.UserType = updatedUser.UserType;

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
        public IActionResult Delete(int id)
        {
            try
            {
                Console.WriteLine($"Deleting user with ID: {id}"); 

                var user = _context.Users.FirstOrDefault(u => u.Id == id);
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

        public IActionResult UserDonations(int userId)
        {
            var user = _context.Users
             .Include(u => u.Donations)
             .ThenInclude(d => d.Activity) // إذا كنت تريد عرض معلومات الأنشطة أيضًا
             .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View("UserDonations",user.Donations);
        }

    }
}
