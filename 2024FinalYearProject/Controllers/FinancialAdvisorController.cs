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
        public async Task<IActionResult> Advice(string Id)
        {
            var model = await _wrapper.AppUser.GetAdviceInformation(Id);
            model.AppUser = await _userManager.FindByNameAsync(User.Identity.Name);

            return View("Advice", model);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(string clientId, string advisorId, string comment)
        {
            var advice = new Advice()
            {
                clientId = clientId,
                advisorId = advisorId,
                AdviceText = comment
            };

            await _context.Advices.AddAsync(advice);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

    }
}
