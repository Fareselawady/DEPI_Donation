﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public  class Activity
{
    [Key]
    public int ActivityId { get; set; }

    [StringLength(200),Required]
    public required string Title { get; set; }

    public string? Description { get; set; }

    [StringLength(100)]
    public string? Category { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TargetAmount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal CollectedAmount { get; set; } = 0;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [InverseProperty("Activity")]
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    [InverseProperty("Activity")]
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
