using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;
using _2024FinalYearProject.Models.ViewModels.Client;
using _2024FinalYearProject.Models.ViewModels.FinacialAdvisor;

namespace _2024FinalYearProject.Data.Interfaces
{
    public interface IUserRepository : IRepositoryBase<AppUser>
    {
        Task<List<UserViewModel>> GetAllUsersAndBankAccount();
        Task<ClientIndexViewModel> GetUserLandingPageData(string email);
        Task<UserLogInsViewModel> GetUserLogins(string email);
        Task<List<UserAdvisorViewModel>> GetAllUsersAndBankDetails();
        Task<AdviceViewModel> GetAdviceInformation(string userId);
        Task<List<Transaction>> GetUserTransactions(string accountNumber);
    }
}
