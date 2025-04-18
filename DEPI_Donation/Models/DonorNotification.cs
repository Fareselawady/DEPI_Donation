using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

[PrimaryKey("DonorId", "NotificationId")]
public partial class DonorNotification
{
    [Key]
    [Column("DonorID")]
    public int DonorId { get; set; }

    [Key]
    [Column("NotificationID")]
    public int NotificationId { get; set; }

    public bool? IsRead { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("DonorId")]
    [InverseProperty("DonorNotifications")]
    public virtual User Donor { get; set; } = null!;

    [ForeignKey("NotificationId")]
    [InverseProperty("DonorNotifications")]
    public virtual Notification Notification { get; set; } = null!;
}
