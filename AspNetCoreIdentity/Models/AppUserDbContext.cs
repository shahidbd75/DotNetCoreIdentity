using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Models
{
    public class AppUserDbContext: IdentityDbContext<User>
    {
        public AppUserDbContext(DbContextOptions<AppUserDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<User>(user =>user.HasIndex(u => u.Locale).IsUnique(false));

            builder.Entity<Organization>(org =>
            {
                org.HasKey(x => x.Id);
                org.ToTable("Organization");
                org.HasMany<User>().WithOne().HasForeignKey(o => o.OrdId).IsRequired(false);
            });
        }
    }
}
