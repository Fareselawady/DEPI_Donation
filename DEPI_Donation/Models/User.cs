using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DEPI_Donation.Models;

public partial class User : IdentityUser<int>
{

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime")]
    public DateTime? LastLogout { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [InverseProperty("Donor")]
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();

    [InverseProperty("Donor")]
    public ICollection<DonorNotification> DonorNotifications { get; set; } = new List<DonorNotification>();
}
