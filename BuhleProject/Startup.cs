using BuhleProject.Models.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace BuhleProject
{
    public class Startup
    {
        IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDBcontext>(options =>
                    options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
            services.AddTransient<IRepositoryWrapper, RepositoryWrapper>();
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDBcontext>()
            .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                //options.Tokens.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication(); //login things
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });
            AppDBcontext.CreateAdminAccount(app.ApplicationServices, _configuration).Wait(); //admin role things
            //AppDBcontext.seedData(app.ApplicationServices);
        }
    }
}
