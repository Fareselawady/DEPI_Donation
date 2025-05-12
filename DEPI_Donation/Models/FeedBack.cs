using System.ComponentModel.DataAnnotations;

namespace DEPI_Donation.Models;

public class FeedBack
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required, EmailAddress, DataType("Email")]
    public required string Email { get; set; }
    public string? Subject { get; set; }
    [Required]
    public required string Message { get; set; }
}
