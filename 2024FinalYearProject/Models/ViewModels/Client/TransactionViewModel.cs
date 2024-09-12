namespace _2024FinalYearProject.Models.ViewModels.Client
{
    public class TransactionViewModel
    {
        public AppUser AppUser { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string None { get; set; } = "--None--";
    }
}
