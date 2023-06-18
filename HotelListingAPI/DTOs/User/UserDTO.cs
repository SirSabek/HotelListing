using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.DTOs.User
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNember { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(maximumLength: 15, ErrorMessage = "Your {0} must be between {2} and {1} characters.", MinimumLength = 6)]
        public string Password { get; set; } = null!;
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        public ICollection<string> Roles { get; set; }

    }
}
