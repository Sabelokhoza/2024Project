using _2024FinalYearProject.Data;
using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.FinacialAdvisor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2024FinalYearProject.Controllers
{
    public class FinancialAdvisorController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public FinancialAdvisorController(IRepositoryWrapper wrapper, UserManager<AppUser> userManager, AppDbContext context)
        {
            _wrapper = wrapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _wrapper.AppUser.GetAllUsersAndBankDetails();
            var model = new UsersPageViewModel()
            {
                User = await _userManager.FindByNameAsync(User.Identity.Name),
                Users = users
            };
            return View("Users", model);
        }

        [HttpPost]
        public async Task<IActionResult> Advice(int Id)
        {
            var advice = await _wrapper.Advice.GetByIdAsync(Id);
            var model = await _wrapper.AppUser.GetAdviceInformation(advice.clientId);
            model.AppUser = await _userManager.FindByNameAsync(User.Identity.Name);
            model.adviceId = advice.Id;
            return View("Advice", model);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(string clientId, string advisorId, int adviceId, string comment)
        {
            var advice = await _wrapper.Advice.GetByIdAsync(adviceId);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            advice.Message = comment;
            advice.advisorName = user.UserName;
            advice.advisorId = user.Id;

            await _wrapper.Advice.UpdateAsync(advice);

            return RedirectToAction("Available");
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        public async Task<IActionResult> Available()
        {
            var advices = (await _wrapper.Advice.GetAllAsync()).Where(w => w.Message == "" &&
            w.advisorId == null).ToList();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var model = new PendingAdviseModel();
            model.PendingAdvices = advices;
            model.User = user;

            return View("Available", model);
        }

    }
}
