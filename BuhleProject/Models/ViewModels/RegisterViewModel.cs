using System.ComponentModel.DataAnnotations;

namespace BuhleProject.Models.ViewModels
{
    public class RegisterViewModel
    {

        string role = "standard user";
        public string Role { get => role; set => role = value; }
        public RegisterViewModel()
        {
            Role = role;
        }
        [Required]
        [Display(Name = "First Name(s)")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Institution")]
        public string Institution { get; set; }

        public bool Accepted { get; set; }

        [Required]
        [Display(Name = "current address")]
        public string address { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number ")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
