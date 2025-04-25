using DEPI_Donation.Data;
using DEPI_Donation.Models;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models.ModelsBL
{
    public class NotificationBL
    {
        private readonly AppDbcontext _context;

        public NotificationBL(AppDbcontext context)
        {
            _context = context;
        }

        public void AddNotification(string title, string description)
        {
            var notification = new Notification
            {
                Title = title,
                Description = description,
                CreatedAt = DateTime.Now
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }

        public void UpdateNotification(int notificationId, string title, string description)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
            if (notification != null)
            {
                notification.Title = title;
                notification.Description = description;
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Notification not found.");
            }
        }

        public void DeleteNotification(int notificationId)
        {
            var notification = _context.Notifications
                .Include(n => n.DonorNotifications)
                .FirstOrDefault(n => n.NotificationId == notificationId);

            if (notification != null)
            {
                if (notification.DonorNotifications.Any())
                {
                    _context.DonorNotifications.RemoveRange(notification.DonorNotifications);
                }

                _context.Notifications.Remove(notification);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Notification not found.");
            }
        }

        public Notification GetNotificationById(int notificationId)
        {
            var notification = _context.Notifications
                .Include(n => n.DonorNotifications)
                .FirstOrDefault(n => n.NotificationId == notificationId);

            if (notification != null)
            {
                return notification;
            }
            else
            {
                throw new Exception("Notification not found.");
            }
        }

        public List<Notification> GetAllNotifications()
        {
            return _context.Notifications
                .Include(n => n.DonorNotifications)
                .ToList();
        }
    }
}
