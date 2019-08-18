using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Models
{
    public class User: IdentityUser
    {
        public string Locale { get; set; } = "en-US";

        public string OrdId { get; set; }
    }
}
