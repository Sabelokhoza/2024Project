﻿using Microsoft.AspNetCore.Identity;

namespace _2024FinalYearProject.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentStaffNumber { get; set; }
        public string IDnumber { get; set; }
        public string AccountNumber { get; set; }
        public string UserRole { get; set; }
        public string Password { get; set; }
    }
}
