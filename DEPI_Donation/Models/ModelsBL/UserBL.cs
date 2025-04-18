using DEPI_Donation.Data;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models.ModelsBL
{
    public class UserBL
    {
        private readonly AppDbcontext _context;

        public UserBL(AppDbcontext context)
        {
            _context = context;
        }

        public void AddUser(string userName, string email, string phone, string password, DateTime dateOfBirth, string userType)
        {
            var user = new User
            {
                UserName = userName,
                Email = email,
                Phone = phone,
                Password = password,
                CreatedAt = DateTime.Now,
                LastLogout = null,
                DateOfBirth = DateOnly.FromDateTime(dateOfBirth),
                UserType = userType
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

    
        public void UpdateUser(int userId, string userName, string phone, string password, DateTime dateOfBirth)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.UserName = userName;
                user.Phone = phone;
                user.Password = password;
                user.DateOfBirth = DateOnly.FromDateTime(dateOfBirth);

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public User Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                user.LastLogout = null; 
                _context.SaveChanges();
                return user;
            }
            else
            {
                throw new Exception("Invalid credentials.");
            }
        }

        public void Logout(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.LastLogout = DateTime.Now;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        public List<Donation> donations(int userId)
        {
            var user = _context.Users.Include(u => u.Donations).FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                return user.Donations.ToList();
            }
            else
            {
                throw new Exception("User not found.");
            }
        }
    }

}
