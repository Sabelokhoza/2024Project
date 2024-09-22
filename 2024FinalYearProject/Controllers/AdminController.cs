using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using _2024FinalYearProject.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;
using _2024FinalYearProject.Models.ViewModels;
using System.Data;
using _2024FinalYearProject.Models.ViewModels.Client;
using _2024FinalYearProject.Models.ViewModels.Consultant;
using _2024FinalYearProject.Models.ViewModels.FinacialAdvisor;
using Microsoft.AspNetCore.Authorization;

namespace _2024FinalYearProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepositoryWrapper _wrapper;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(IRepositoryWrapper wrapper , UserManager<AppUser> userManager )
        {
            _wrapper = wrapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var transactions = await _wrapper.Transaction.GetAllAsync();
            var consultants =  (await _userManager.GetUsersInRoleAsync("Consultant")).ToList();
            var users =  (await _userManager.GetUsersInRoleAsync("User")).ToList();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var indexPageViewModel = new IndexPageViewModel()
            {
                Transactions = transactions ,
                Consultants = consultants,
                Users = users,
                User = user

            };

            return View(indexPageViewModel);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Password(string Id)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var client = await _userManager.FindByIdAsync(Id);

            PasswordResetViewModel model = new PasswordResetViewModel();
            model.AppUser = user;
            model.Client = client;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Password(string Id, string Password, string ConfirmPassword)
        {
            var model = new PasswordResetViewModel()
            {
                AppUser = await _userManager.FindByNameAsync(User.Identity.Name),
                Client = await _userManager.FindByIdAsync(Id)
            };
            if (ModelState.IsValid)
            {
                if (Password != ConfirmPassword || string.IsNullOrEmpty(ConfirmPassword) || string.IsNullOrEmpty(Password))
                {
                    ModelState.AddModelError("", "Password and Confirm Password must match");

                    return View("Password", model);
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(Id);
                    IdentityResult validPass = null;
                    if (!string.IsNullOrEmpty(Password))
                    {
                        if (await _userManager.HasPasswordAsync(user))
                        {
                            await _userManager.RemovePasswordAsync(user);
                        }

                        validPass = await _userManager.AddPasswordAsync(user, Password);

                        if (!validPass.Succeeded)
                        {
                            AddErrorsFromResult(validPass);
                        }
                    }

                    if ((validPass == null)
                          || (Password != string.Empty && validPass.Succeeded))
                    {
                        IdentityResult result = await _userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            Notification notification = new Notification()
                            {
                                UserEmail = user.Email,
                                NotificationDate = DateTime.Now,
                                Message = "Your password has been reset successfully by consultant"

                            };

                            await _wrapper.Notification.AddAsync(notification);
                            ModelState.AddModelError("", "Password has been reset successfully");
                            return RedirectToAction("Password", model);
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }


            }
            return View("Password", model);
        }

        [HttpGet]
        public async Task<IActionResult> Account(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            var model = new UpdateViewModel()
            {
                UserRole = user.UserRole,
                Email = user.Email,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IdNumber = user.IDnumber,
                FirstName = user.FirstName,
                AppUserId = user.Id,
                StudentStaffNumber = user.StudentStaffNumber

            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Account(UpdateViewModel user)
        {
            var model = await _userManager.FindByIdAsync(user.AppUserId);

            model.UserRole = user.UserRole;
            model.Email = user.Email;
            model.LastName = user.LastName;
            model.PhoneNumber = user.PhoneNumber;
            model.IDnumber = user.IdNumber;
            model.FirstName = user.FirstName;
            model.StudentStaffNumber = user.StudentStaffNumber;
            var result = await _userManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                Notification notification = new Notification()
                {
                    UserEmail = model.Email,
                    NotificationDate = DateTime.Now,
                    Message = "Your account has been updated successfully by admin"
                };
                await _wrapper.Notification.AddAsync(notification);
                return RedirectToAction("Users");
            }

            return View(user);

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

        public async Task<IActionResult> Wallet(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            var model = await _wrapper.AppUser.GetUserLandingPageData(user.Email);
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Reports(string Id)
        {
            var reports = await _wrapper.report.GetReports(Id);
            return View(reports);
        }

        [HttpPost]
        public async Task<IActionResult> Advices(string Id)
        {
            var advices = (await _wrapper.Advice.GetAdvices()).Where( w => w.Advice.advisorId == Id ).ToList();
            return View(advices);
        }


        [HttpGet]
        public async Task<IActionResult> AddStaff()
        {
            return View();
        }

        public async Task<IActionResult> AddStaff(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
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
                while (_userManager.Users.Where(u => u.AccountNumber != _randomAccount).FirstOrDefault() == null);
                user.AccountNumber = _randomAccount;



                IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, user.UserRole);
                    BankAccount bankAccountMain = new()
                    {
                        AccountNumber = _randomAccount,
                        Balance = 600m,
                        BankAccountType = "Main",
                        UserEmail = user.Email,
                    };
                    await _wrapper.BankAccount.AddAsync(bankAccountMain);

                        Notification notification = new Notification();
                        notification.UserEmail = user.Email;
                        notification.NotificationDate = DateTime.Now;
                        notification.Message = "Welcome to UniBank , you have been added by admin as a "+ user.UserRole ;

                        await _wrapper.Notification.AddAsync(notification);

                        return RedirectToAction("Staff");
                    }
                }
            return View(registerModel);
        }


        [HttpGet]
        public async Task<IActionResult> Staff()
        {


            var users = await _wrapper.AppUser.GetStaffMembers();
            var model = new StaffViewModel()
            {
                users = users,
            };

            return View(model);
        }

       
        [HttpGet]
        public async Task<IActionResult> Consultants()
        {


            var users =( await _userManager.GetUsersInRoleAsync("Consultant")).ToList();

            return View(users);
        }

        //delete transaction
        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _wrapper.Transaction.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> DepositWithdraw(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return View(new ConsultantDepositModel
                {
                    AccountNumber = user.AccountNumber,
                    UserEmail = user.Email,
                });
            }
            return RedirectToAction("Index", "Consultant");
        }

        [HttpPost]
        public async Task<IActionResult> DepositWithdraw(ConsultantDepositModel model, string action)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.UserEmail);
                if (user != null)
                {
                    var AllBankAcc = await _wrapper.BankAccount.GetAllAsync();
                    var userBankAcc = AllBankAcc.FirstOrDefault(bc => bc.UserEmail == user.Email);
                    if (userBankAcc != null)
                    {
                        if (action.ToLower() == "deposit")
                        {
                            userBankAcc.Balance += model.Amount;
                        }
                        else
                        {
                            if (userBankAcc.Balance - model.Amount < -50)
                            {
                                ModelState.AddModelError("", "User has insuffecient balance in their account");
                                return View(model);
                            }
                            userBankAcc.Balance -= model.Amount;
                        }
                        await _wrapper.BankAccount.UpdateAsync(userBankAcc);
                        var transaction = new Transaction
                        {
                            Amount = model.Amount,
                            UserEmail = model.UserEmail,
                            Reference = $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action)} cash in account",
                            BankAccountIdReceiver = int.Parse(user.AccountNumber),
                            BankAccountIdSender = 0,
                            TransactionDate = DateTime.Now
                        };
                        await _wrapper.Transaction.AddAsync(transaction);
                        _wrapper.SaveChanges();
                        Message = $"Money Successfully {CultureInfo.CurrentCulture.TextInfo.ToTitleCase(action)} to account";
                        return RedirectToAction("Index", "Consultant");
                    }
                    else
                    {
                        Message = "Couldn't find bank account, please contact system administrator";
                        ModelState.AddModelError("", "Couldn't find bank account");
                    }
                }
                else
                {
                    Message = "Couldn't find user, please contact system administrator";
                    ModelState.AddModelError("", "Couldn't find user");
                }
            }
            return View(model);
        }


        [TempData]
        public string Message { get; set; }

    

        public async Task<IActionResult> UserReport(string Id)
        {
            var client = await _userManager.FindByIdAsync(Id);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var report = new Report()
            {
                Message = $"Generated {client.UserName} transaction report information",
                userName = user.UserName
            };

            await  _wrapper.report.AddAsync(report);
            return RedirectToAction("Users");

        }

        public async Task<IActionResult> UsersReport()
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var report = new Report()
                {
                    Message = "Generated all users report and their account balance information",
                    userName = user.UserName
                };
                await _wrapper.report.AddAsync(report);
                var users = await _wrapper.AppUser.GetAllUsersAndBankDetails();

                var html = @"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Users Bank Details Report</title>
                    <style>
                        body { font-family: Arial, sans-serif; margin: 0; padding: 20px; }
                        h1 { color: #333; text-align: center; }
                        table { width: 100%; border-collapse: collapse; margin-top: 20px; }
                        th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }
                        th { background-color: #f2f2f2; color: #333; font-weight: bold; }
                        tr:nth-child(even) { background-color: #f9f9f9; }
                        tr:hover { background-color: #f5f5f5; }
                    </style>
                </head>
                <body>
                    <h1>Users Bank Details Report</h1>
                    <table>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Surname</th>
                                <th>Email</th>
                                <th>Balance</th>
                                <th>Money In</th>
                                <th>Money Out</th>
                            </tr>
                        </thead>
                        <tbody>";

                foreach (var _user in users)
                {
                    html += $@"
                            <tr>
                                <td>{_user.AppUser.FirstName}</td>
                                <td>{_user.AppUser.LastName}</td>
                                <td>{_user.AppUser.Email}</td>
                                <td>R {_user.BankAccount.Balance}</td>
                                <td>R {_user.MoneyIn}</td>
                                <td>R {_user.MoneyOut}</td>
                            </tr>";
                }

                html += @"
                        </tbody>
                    </table>
                </body>
                </html>";

                var fileName = "users_bank_details.html";
                var reportBytes = Encoding.UTF8.GetBytes(html);
                return File(reportBytes, "text/html", fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("An error occurred during report generation.");
            }
        }

       
        public async Task<IActionResult> Reviews()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var feedbacks = await _wrapper.Review.GetAllAsync();
            var model = new FeedbackViewModel()
            {
                feedBacks = feedbacks,
                appUser = user
            };
            return View(model);
        }

        public async Task<IActionResult> UserLogins(string Id)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var client = await _userManager.FindByIdAsync(Id);
            var model = await _wrapper.AppUser.GetUserLogins(client.Email);
            model.AppUser = user;
            return View(model);

        }

        public async Task<IActionResult> Transactions(string Id)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var client = await _userManager.FindByIdAsync(Id);
            var transactions = (await _wrapper.Transaction.GetAllAsync()).
                Where(t => int.Parse(client.AccountNumber) == t.BankAccountIdReceiver ||
                t.BankAccountIdSender == int.Parse(client.AccountNumber)).OrderByDescending(o => o.TransactionDate).ToList();

            var model = new Models.ViewModels.Consultant.TransactionsViewModel
            {
                Transactions = transactions,
                AppUser = user,
                Client = client
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Reverse(int Id, string userId)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var transaction = await _wrapper.Transaction.GetByIdAsync(Id);
            var person = await _userManager.FindByIdAsync(userId);
            var accountSender = (await _wrapper.BankAccount.GetAllAsync()).FirstOrDefault(w => int.Parse(w.AccountNumber) == transaction.BankAccountIdSender);
            var accountReciever = (await _wrapper.BankAccount.GetAllAsync()).FirstOrDefault(w => int.Parse(w.AccountNumber) == transaction.BankAccountIdReceiver);

            if (accountReciever != null)
            {
                accountReciever.Balance -= transaction.Amount;
                await _wrapper.BankAccount.UpdateAsync(accountReciever);
            }

            if (accountSender != null)
            {
                accountSender.Balance += transaction.Amount;

                await _wrapper.BankAccount.UpdateAsync(accountSender);
            }



            var transaction1 = new Transaction()
            {
                Amount = transaction.Amount,
                UserEmail = transaction.UserEmail,
                TransactionDate = DateTime.Now,
                BankAccountIdReceiver = transaction.BankAccountIdSender,
                BankAccountIdSender = transaction.BankAccountIdReceiver,
                TrandactionType = "Reversal"

            };
            await _wrapper.Transaction.AddAsync(transaction1);
            return RedirectToAction("Transactions", person.Id);

        }

        public async Task<IActionResult> ViewAllLogins(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var allLogins = await _wrapper.Logins.GetAllAsync();
                var userBankAccount = (await _wrapper.BankAccount.GetAllAsync()).FirstOrDefault(bc => bc.AccountNumber == user.AccountNumber);
                return View(new ConsultantViewModel
                {
                    SelectedUser = user,
                    loginSessions = allLogins.Where(u => u.UserEmail == email).OrderBy(l => l.TimeStamp)
                });
            }
            return View("Index");


        }


        [HttpPost]
        public async Task<IActionResult> Withdraw(string Id , decimal amount)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByIdAsync(Id);
            var model = await _wrapper.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            if (bankAccount.Balance >= amount)
            {
                bankAccount.Balance = bankAccount.Balance - amount;
                await _wrapper.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Withdrawal";
                transaction.BankAccountIdSender = int.Parse(bankAccount.AccountNumber);
                await _wrapper.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have withdrawn R" + amount + " from your account on " + DateTime.Now;

                await _wrapper.Notification.AddAsync(notification);
            }
            return RedirectToAction("Wallet", new { id = user.Id });

        }


        [HttpPost]
        public async Task<IActionResult> Deposit(string Id , decimal amount)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByIdAsync(Id);
            var model = await _wrapper.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            if (amount > 0)
            {
                bankAccount.Balance = bankAccount.Balance + amount;
                await _wrapper.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Deposit";
                transaction.BankAccountIdReceiver = int.Parse(bankAccount.AccountNumber);
                await _wrapper.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have deposited R" + amount + " to your account on " + DateTime.Now;

                await _wrapper.Notification.AddAsync(notification);
            }

            return RedirectToAction("Wallet", new { id = user.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Transfer( string Id ,int amount, int accountNo)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByIdAsync(Id);
            var model = await _wrapper.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            var recieverAcc = (await _wrapper.BankAccount.GetAllAsync()).Where(x => x.AccountNumber == accountNo.ToString() && x.BankAccountType == "Main").FirstOrDefault();

            if (amount > 0)
            {
                if (recieverAcc is not null)
                {
                    recieverAcc.Balance = recieverAcc.Balance + amount;
                    await _wrapper.BankAccount.UpdateAsync(recieverAcc);
                }

                bankAccount.Balance = bankAccount.Balance - amount;
                await _wrapper.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Transfer";
                transaction.BankAccountIdReceiver = accountNo;
                transaction.BankAccountIdSender = int.Parse(bankAccount.AccountNumber);
                await _wrapper.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have transerred R" + amount + " to bank account number :" + bankAccount.AccountNumber + " on " + DateTime.Now;

                await _wrapper.Notification.AddAsync(notification);
            }

            return RedirectToAction("Wallet", new { id = user.Id });
        }


        [HttpGet]
        public async Task<IActionResult> GenerateReport()
        {
            try
            {
                List<string> data = new List<string>();
                var reportContent = $"Banking Application\n{DateTime.Now:yyyyMMddHHmmss}\n\n" +
                    $"***Users\\Clients***\n" +
                    $"=====================\n" +
                    $"Account No\tFirst Name\tLast Name\tEmail Address\tStudent Number\n\n";
                var report = _userManager.Users;
                foreach (var u in report)
                {
                    if (await _userManager.IsInRoleAsync(u, "User"))
                    {
                        data.Add($"{u.AccountNumber}\t{u.FirstName}\t{u.LastName}\t{u.Email}\t{u.StudentStaffNumber}\n");
                    }
                }
                reportContent += string.Join('\n', data.ToArray());


                reportContent += $"\n***All Transactions***\n" +
                                 $"==========================\n" +
                    $"Account No\tFirst Name\tLast Name\tEmail Address\tStudent Number\n\n";
                var transactions = await _wrapper.Transaction.GetAllAsync();
                reportContent += string.Join('\n', transactions.Select(u => $"{u.UserEmail}\t{u.Amount}\t{u.BankAccountIdReceiver}\t{u.BankAccountIdSender}\n").ToArray());

                var contentBytes = Encoding.UTF8.GetBytes(reportContent);
                var fileName = $"Report_{DateTime.Now:yyyyMMddHHmmss}.txt";

                return File(contentBytes, "text/plain", fileName);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generating report: {ex.Message}");
            }
        }




        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

    }
}
