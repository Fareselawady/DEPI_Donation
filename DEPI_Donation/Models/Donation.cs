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
    Failed
}

public partial class Donation
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("DonorID")]
    public int? DonorId { get; set; }

    [Column("PaymentID")]
    public int? PaymentId { get; set; }

    [Column("ActivityID")]
    public int? ActivityId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

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
