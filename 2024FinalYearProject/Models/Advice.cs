namespace _2024FinalYearProject.Models
{
    public class Advice
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Message { get; set; }
        public string clientId { get; set; }
        public string clientName { get; set; }
        public string? advisorId { get; set; }
        public string? advisorName { get; set; }
        public double Amount { get; set; }
        public double Income { get; set; }
        public double Expenses { get; set; }
    }
}
