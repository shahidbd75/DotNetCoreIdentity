using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
