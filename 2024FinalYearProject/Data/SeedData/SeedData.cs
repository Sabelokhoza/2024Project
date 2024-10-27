using _2024FinalYearProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace _2024FinalYearProject.Data.SeedData
{
    public static class SeedData
    {
        private static readonly string password = "@Banker123";
        private static readonly AppUser consultantUser = new AppUser
        {
            UserName = "def_consultant",
            FirstName = "Nick",
            LastName = "Cage",
            Email = "nicky@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "9876543210123",
            StudentStaffNumber = "0123456789",
            AccountNumber = "0000000001",
            UserRole = "Consultant"
        }; 
        private static readonly AppUser AdvisorUser = new AppUser
        {
            UserName = "def_advisor",
            FirstName = "Katlegp",
            LastName = "Khoza",
            Email = "khoza@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "98765430123",
            StudentStaffNumber = "012356789",
            AccountNumber = "0000000002",
            UserRole = "Advisor"
        };
        private static readonly AppUser adminUser = new AppUser
        {
            UserName = "def_admin",
            FirstName = "Kamogelo",
            LastName = "Khumalo",
            Email = "khumalo@ufs.ac.za",
            DateOfBirth = DateTime.Now,
            IDnumber = "98765430125",
            StudentStaffNumber = "012356787",
            AccountNumber = "0000000003",
            UserRole = "Admin"
        };
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices.CreateScope()
               .ServiceProvider.GetRequiredService<AppDbContext>();

            UserManager<AppUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();

            if (await userManager.FindByNameAsync(consultantUser.UserName) == null)
            {

                if (await roleManager.FindByNameAsync(consultantUser.UserRole) == null)
                    await roleManager.CreateAsync(new(consultantUser.UserRole));

                if (await roleManager.FindByNameAsync(AdvisorUser.UserRole) == null)
                    await roleManager.CreateAsync(new(AdvisorUser.UserRole));

                AppUser user = new()
                {
                    UserName = consultantUser.UserName,
                    FirstName = consultantUser.FirstName,
                    LastName = consultantUser.LastName,
                    Email = consultantUser.Email,
                    StudentStaffNumber = consultantUser.StudentStaffNumber,
                    AccountNumber = consultantUser.AccountNumber,
                    DateOfBirth = consultantUser.DateOfBirth,
                    IDnumber = consultantUser.IDnumber,
                    UserRole = consultantUser.UserRole,
                    Password = password
                }; 
                AppUser user2 = new()
                {
                    UserName = AdvisorUser.UserName,
                    FirstName = AdvisorUser.FirstName,
                    LastName = AdvisorUser.LastName,
                    Email = AdvisorUser.Email,
                    StudentStaffNumber = AdvisorUser.StudentStaffNumber,
                    AccountNumber = AdvisorUser.AccountNumber,
                    DateOfBirth = AdvisorUser.DateOfBirth,
                    IDnumber = AdvisorUser.IDnumber,
                    UserRole = AdvisorUser.UserRole,
                    Password = password
                }; AppUser user3 = new()
                {
                    UserName = adminUser.UserName,
                    FirstName = adminUser.FirstName,
                    LastName = adminUser.LastName,
                    Email = adminUser.Email,
                    StudentStaffNumber = adminUser.StudentStaffNumber,
                    AccountNumber = adminUser.AccountNumber,
                    DateOfBirth = adminUser.DateOfBirth,
                    IDnumber = adminUser.IDnumber,
                    UserRole = adminUser.UserRole,
                    Password = password
                };
                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Consultant");              
                IdentityResult result2 = await userManager.CreateAsync(user2, password);
                if (result2.Succeeded)
                    await userManager.AddToRoleAsync(user2, "Advisor");  
                IdentityResult result3 = await userManager.CreateAsync(user3, password);
                if (result3.Succeeded)
                    await userManager.AddToRoleAsync(user3, "Admin");
            }
        }
    }
}