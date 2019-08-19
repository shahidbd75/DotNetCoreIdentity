using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentity.DataAccess
{
    public class CustomUserClaimsPrincipalFactory: UserClaimsPrincipalFactory<User>
    {
        public CustomUserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var claimIdentity = await base.GenerateClaimsAsync(user);
            claimIdentity.AddClaim(new Claim("Locale",user.Locale));

            return claimIdentity;
        }
    }
}
