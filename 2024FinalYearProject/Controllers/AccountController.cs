using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels;
using _2024FinalYearProject.Models.ViewModels.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2024FinalYearProject.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepositoryWrapper wrapper;
        private readonly string role = "User";

        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager,
            RoleManager<IdentityRole> _roleManager, IRepositoryWrapper _wrapper)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            wrapper = _wrapper;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string registerAs = "student")
        {
            return View(new RegisterViewModel() { RegisterAs = registerAs });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new(role));

                AppUser user = new()
                {
                    UserName = (registerModel.LastName + "-" + registerModel.FirstName[0]),
                    IDnumber = registerModel.IdPassportNumber,
                    Email = registerModel.EmailAddress,
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    StudentStaffNumber = registerModel.StudentStaffNumber,
                    UserRole = registerModel.RegisterAs
                };

                Random rndAccount = new Random();
                string _randomAccount = string.Empty;
                do
                {
                    _randomAccount = rndAccount.Next(99999999, 999999999).ToString();
                }
                while (userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
                user.AccountNumber = _randomAccount;



                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    BankAccount bankAccountMain = new()
                    {
                        AccountNumber = _randomAccount,
                        Balance = 150m,
                        BankAccountType = "Main",
                        UserEmail = user.Email,
                    };
                    await wrapper.BankAccount.AddAsync(bankAccountMain);
                  
                    var signin_result = await signInManager.PasswordSignInAsync(user, registerModel.Password,
                        isPersistent: false, lockoutOnFailure: false);
                    if (signin_result.Succeeded)
                    {
                        var newLogin = new LoginSessions
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await wrapper.Logins.AddAsync(newLogin);
                        wrapper.SaveChanges();

                        Transaction transaction = new()
                        {
                            BankAccountIdReceiver = int.Parse(_randomAccount),
                            Amount = 150m,
                            TrandactionType = "Signing fee",
                            UserEmail = user.Email,
                        };
                        await wrapper.Transaction.AddAsync(transaction);

                        Notification notification = new Notification();
                        notification.UserEmail = user.Email;
                        notification.NotificationDate = DateTime.Now;
                        notification.Message = "Welcome to UniBank , your account no is :"+ _randomAccount;

                        await wrapper.Notification.AddAsync(notification);

                        Notification notification1 = new Notification();
                        notification1.UserEmail = user.Email;
                        notification1.NotificationDate = DateTime.Now;
                        notification1.Message = "Welcome to UniBank , you have recieved R150 siging fee";

                        await wrapper.Notification.AddAsync(notification);

                        return RedirectToAction("Wallet", "Client");
                    }
                }
                else
                    foreach (var error in result.Errors.Select(e => e.Description))
                        ModelState.AddModelError("", error);
            }
            return View(registerModel);
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync
                        (user, model.Password, isPersistent: model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var newLogin = new LoginSessions
                        {
                            TimeStamp = DateTime.Now,
                            UserEmail = user.Email,
                        };
                        await wrapper.Logins.AddAsync(newLogin);
                        wrapper.SaveChanges();


                        if (await userManager.IsInRoleAsync(user, "User"))
                        {
                            return RedirectToAction("Wallet", "Client");
                        }


                        if (await userManager.IsInRoleAsync(user, "Consultant"))
                        {
                            return RedirectToAction("Index", "Consultant");
                        }

                        if (await userManager.IsInRoleAsync(user, "Admin"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }

                        if (await userManager.IsInRoleAsync(user, "Advisor"))
                        {
                            return RedirectToAction("Index", "FinancialAdvisor");
                        }

                        return Redirect(model?.ReturnUrl ?? "/Home/Index");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}