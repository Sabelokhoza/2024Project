using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;

namespace _2024FinalYearProject.Data.Interfaces
{
    public interface IReportRepository : IRepositoryBase<Report>
    {
        Task<ReportViewModel> GetReports(string Id);
    }
}
