using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;

namespace _2024FinalYearProject.Data.Interfaces
{
    public interface IAdviceRepository : IRepositoryBase<Advice>
    {
        Task<List<AdviceModel>> GetAdvices();  
    }
}
