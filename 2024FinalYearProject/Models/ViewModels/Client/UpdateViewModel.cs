using System.ComponentModel.DataAnnotations;

namespace _2024FinalYearProject.Models.ViewModels.Client
{
    public class UpdateViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string AccountNumber { get; set; }
        public string AppUserId { get; set; }
        [Required]
        public string IdNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserRole { get; set; }
        [Required]
        public string StudentStaffNumber{ get; set; }
    }

}
