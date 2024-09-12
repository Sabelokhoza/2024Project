namespace _2024FinalYearProject.Models.ViewModels.FinacialAdvisor
{
    public class UserAdvisorViewModel
    {
        public AppUser AppUser { get; set; }
        public BankAccount BankAccount { get; set; } = new BankAccount();
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }
    }

    public class UsersPageViewModel
    {
        public AppUser User { get; set; }
        public List<UserAdvisorViewModel    > Users { get; set; }
    }
         
}
