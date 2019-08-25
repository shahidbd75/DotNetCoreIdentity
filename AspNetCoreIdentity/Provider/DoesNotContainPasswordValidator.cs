using System;
using System.Threading.Tasks;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Provider
{
    public class DoesNotContainPasswordValidator<TUser> : IPasswordValidator<User> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            var username = await manager.GetUserNameAsync(user);

            if (username == password)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password doesn't contain username"});
            }

            if (password.Contains("password"))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password doesn't contain password" });
            }
            return IdentityResult.Success;
        }
    }
}
