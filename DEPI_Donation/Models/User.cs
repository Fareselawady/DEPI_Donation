using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;
public enum UserType
{
    Donor,
    Admin
}
[Table("User")]
[Index("Email", Name = "UQ__User__A9D10534AF242973", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string? UserName { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Password { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLogout { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(10)]
    public UserType UserType { get; set; }

    [InverseProperty("Donor")]
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    [InverseProperty("Donor")]
    public virtual ICollection<DonorNotification> DonorNotifications { get; set; } = new List<DonorNotification>();
}
