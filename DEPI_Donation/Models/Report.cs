using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public  class Report
{
    [Key]
    public int ReportId { get; set; }

    [StringLength(200),Required]
    public required string Title { get; set; }

    public string? Description { get; set; }

    [Required]
    public required int ActivityId { get; set; }

    [ForeignKey("ActivityId")]
    [InverseProperty("Reports")]
    [Required]
    public required virtual Activity Activity { get; set; }
}
