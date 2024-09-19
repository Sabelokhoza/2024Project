using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace _2024FinalYearProject.Data
{
    public class AdviceRepository : RepositoryBase<Advice>, IAdviceRepository
    {
        private readonly AppDbContext _context;

        public AdviceRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AdviceModel>> GetAdvices()
        {
           return await( from a in _context.Advices
                          join u in _context.Users
                          on a.advisorId  equals  u.Id
                          join u2 in _context.Users
                          on a.clientId equals u2.Id
                          select new AdviceModel 
                          { 
                              Client = u2,
                              Advice = a ,
                              Consultant = u 
                          }).ToListAsync();
        }
    }
}
