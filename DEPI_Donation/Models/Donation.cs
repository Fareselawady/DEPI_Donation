using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public enum DonationStatusType
{
    Pending,
    Confirmed,
    Canceled
}

public partial class Donation
{
    [Key]
    public int DonationId { get; set; }

    public int? DonorId { get; set; }

    public int? PaymentId { get; set; }

    public int? ActivityId { get; set; }

    [Column(TypeName = "decimal(18, 2)"), Required]
    public required decimal Amount { get; set; }

    public DateOnly? DonationDate { get; set; }

    public DonationStatusType Status { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("Donations")]
    public virtual Activity? Activity { get; set; }

    [ForeignKey("DonorId")]
    [InverseProperty("Donations")]
    public virtual User? Donor { get; set; }

    [ForeignKey("PaymentId")]
    [InverseProperty("Donations")]
    public virtual Payment? Payment { get; set; }
}
