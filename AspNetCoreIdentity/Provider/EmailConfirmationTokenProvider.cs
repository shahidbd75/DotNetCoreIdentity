using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentity.Provider
{
    public class EmailConfirmationTokenProvider<TUser>: DataProtectorTokenProvider<User> where TUser:class 
    {
        public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<EmailDataProtectionOptions> options) : base(dataProtectionProvider, options)
        {
        }
    }

    public class EmailDataProtectionOptions : DataProtectionTokenProviderOptions
    {

    }
}
