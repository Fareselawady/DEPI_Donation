using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

[Table("Notification")]
public partial class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Notification")]
    public virtual ICollection<DonorNotification> DonorNotifications { get; set; } = new List<DonorNotification>();
}
