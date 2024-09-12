namespace _2024FinalYearProject.Models.ViewModels.Consultant
{
    public class TransactionsViewModel
    {
        public AppUser AppUser { get; set; }
        public AppUser Client { get; set; }
        public List<Transaction> Transactions { get; set;}
        public string None { get; set; } = "--None--";
    }
}
