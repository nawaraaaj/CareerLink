using System.ComponentModel.DataAnnotations;

namespace CarrerLink.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EmailName { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6-20")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6-20")]
        [Compare("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }

        public string Mobile { get; set; }
    }
}
