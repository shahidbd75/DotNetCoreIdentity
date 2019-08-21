using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModel
{
    public class RegisterViewModel
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email Address")]
        public string Email { get; set; }
    }
}
