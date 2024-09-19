namespace _2024FinalYearProject.Models.ViewModels.Admin
{
    public class AdviceModel
    {
        public Advice Advice { get; set; }
        public AppUser Consultant { get; set; }
        public AppUser Client { get; set; }
    }
}
