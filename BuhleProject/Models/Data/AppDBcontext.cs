using BuhleProject.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;


namespace BuhleProject.Models.Data
{
    public class AppDBcontext : IdentityDbContext<IdentityUser>
    {
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<User> Userss { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Document> Documents { get; set; }
         
        public AppDBcontext(DbContextOptions<AppDBcontext> options) : base(options)
        {
        }


 

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            UserManager<IdentityUser> userManager =
                serviceProvider.GetRequiredService<UserManager<IdentityUser>>();  //dependency injection
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();  //dep inj
            string username = configuration["Data:AdminUser:UserName"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
                IdentityUser user = new IdentityUser
                {
                    UserName = username
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
           
        }//end of admin seeding


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoanApplication>(); 
            modelBuilder.Entity<User>(); 
            modelBuilder.Entity<Document>();
            modelBuilder.Entity<Loan>();
            base.OnModelCreating(modelBuilder);
        }


    }
}
