using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModel
{
    public class TwoFactorModel
    {
        [Required]
        public string Token { get; set; }
    }
}
