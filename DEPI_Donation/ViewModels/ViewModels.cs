using System.ComponentModel.DataAnnotations;
namespace DEPI_Donation.ViewModels
{
    public class RegisterViewModel
    {
        public string? UserName { get; set; }

        [DataType(DataType.EmailAddress), EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.PhoneNumber),Phone]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [DataType(DataType.Password),Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        public required string Email { get; set; }
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        //public bool RememberMe { get; set; }
    }
}