using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DEPI_Donation.Models;

public partial class Payment
{
    [Key]
    public int PaymentId { get; set; }

    [StringLength(50)]
    public string? PaymentMethod { get; set; }

    [InverseProperty("Payment")]
    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
