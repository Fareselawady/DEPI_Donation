using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public partial class Report
{
    [Key]
    [Column("ReportID")]
    public int ReportId { get; set; }

    [StringLength(200)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [InverseProperty("Report")]
    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
