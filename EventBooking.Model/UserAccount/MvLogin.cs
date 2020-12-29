using System.ComponentModel.DataAnnotations;

namespace EventBooking.Model.UserAccount
{
    public class MvLogin
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email required")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "The password must be at least 4 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
