using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public partial class Activity
{
    [Key]
    [Column("ActivityID")]
    public int ActivityId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TargetAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? CollectedAmount { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Column("ReportID")]
    public int? ReportId { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    [ForeignKey("ReportId")]
    [InverseProperty("Activities")]
    public virtual Report? Report { get; set; }
}
