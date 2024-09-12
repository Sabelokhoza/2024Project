namespace _2024FinalYearProject.Models.ViewModels.Client
{
    public class ClientIndexViewModel
    {
        public AppUser user { get; set; }
        public List<Transaction> transactions { get; set; }
        public decimal moneyIn { get; set; }
        public decimal moneyOut { get; set; }
        public string Date { get; set; }
        public BankAccount bankAccount { get; set; }
        public string None { get; set; } = "--None--";
        public BankAccount SavingAccount { get; set; }

    }
}
