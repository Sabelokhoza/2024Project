using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace _2024FinalYearProject.Data
{
    public class ReportRepository : RepositoryBase<Report>, IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ReportViewModel> GetReports(string Id)
        {
            var result = await (from report in _context.Reports
                          join _user in _context.Users
                          on report.consultantId equals _user.Id
                          where report.consultantId == Id
                          select report).ToListAsync();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == Id);
            var model = new ReportViewModel()
            {
                reports = result,
                AdviserName = user.FirstName
            };

            return model;
        }
    }
}
