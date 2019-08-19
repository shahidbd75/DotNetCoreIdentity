using System.Reflection;
using AspNetCoreIdentity.DataAccess;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIdentity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

//            services.AddIdentityCore<User>(options => { });

           // services.AddAuthentication("cookies").AddCookie("cookies", (options) => options.LoginPath = "/Home/Login");

            //services.AddScoped<IUserStore<User>, UserRepository>();
            //services.AddScoped<IUserStore<IdentityUser>, CustomIdentityUserRepository>();

            //For EntityFrameworkCore

            string connectionString ="Data Source=.;Initial Catalog=IdentityUserDb;Integrated Security=True";
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            //services.AddIdentityCore<User>(options => { });

            services.AddIdentity<User, IdentityRole>(options => { }).AddEntityFrameworkStores<AppUserDbContext>();
            services.ConfigureApplicationCookie(config => { config.LoginPath = "/Home/Login"; });

            services.AddDbContext<AppUserDbContext>(option => option.UseSqlServer(connectionString,sql => sql.MigrationsAssembly(migrationAssembly)));
            services.AddScoped<IUserStore<User>, UserOnlyStore<User, AppUserDbContext>>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
