using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
