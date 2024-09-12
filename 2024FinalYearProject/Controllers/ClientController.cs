using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _2024FinalYearProject.Controllers
{
    public class ClientController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<AppUser> _userManager;

        public ClientController(IRepositoryWrapper repo, UserManager<AppUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLandingPageData(user.Email);
            return View(model);

        }

        //User Logins Page
        public async Task<IActionResult> UserLogins()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLogins(user.Email);
            model.AppUser = user;
            return View(model);

        } 
        public async Task<IActionResult> Messages()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var messages = await _repo.Notification.GetUserMessages(user.Email);
            var model = new MessagesViewModel()
            {
                Messages = messages,
                AppUser = user
            };
            return View(model);

        }

        public async Task<IActionResult> Transactions()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var transactions = await _repo.AppUser.GetUserTransactions(user.AccountNumber);
            var model = new TransactionViewModel()
            {
                Transactions = transactions,
                AppUser = user
            };
            return View(model);

        }

        //Manage Account Page

        public async Task<IActionResult> Account()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            var model = new UpdateViewModel()
            {
                AccountNumber = user.AccountNumber,
                UserRole = user.UserRole,
                Email = user.Email,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                IdNumber = user.IDnumber,
                FirstName = user.FirstName,
                AppUserId = user.Id
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Account(UpdateViewModel user)
        {

            //todo.. defensive programming.
            var username = User.Identity.Name;

            var model = await _userManager.FindByNameAsync(username);

            model.AccountNumber = user.AccountNumber;
            model.UserRole = user.UserRole;
            model.Email = user.Email;
            model.LastName = user.LastName;
            model.DateOfBirth = user.DateOfBirth;
            model.PhoneNumber = user.PhoneNumber;
            model.IDnumber = user.IdNumber;
            model.FirstName = user.FirstName;
            await _repo.AppUser.UpdateAsync(model);

            return View(user);

        }


        public async Task<IActionResult> Wallet()
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLandingPageData(user.Email);
            return View(model);

        }

        public async Task<IActionResult> DeleteLogin(int Id)
        {
            await _repo.Logins.RemoveAsync(Id);
            return RedirectToAction("UserLogins");

        }
        public async Task<IActionResult> DeleteMessage(int Id)
        {
            await _repo.Notification.RemoveAsync(Id);
            return RedirectToAction("Messages");

        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(int amount)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            if (bankAccount.Balance >= amount)
            {
                bankAccount.Balance = bankAccount.Balance - amount;
                await _repo.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Withdrawal";
                transaction.BankAccountIdSender = int.Parse( bankAccount.AccountNumber);
                await _repo.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have withdrawn R" + amount + " from your account on " + DateTime.Now;

                await _repo.Notification.AddAsync(notification);
            }
            return RedirectToAction("Wallet");

        }


        [HttpPost]
        public async Task<IActionResult> Deposit(int amount)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            if (amount > 0)
            {
                bankAccount.Balance = bankAccount.Balance + amount;
                await _repo.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Deposit";
                transaction.BankAccountIdReceiver = int.Parse(bankAccount.AccountNumber);
                await _repo.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have deposited R" + amount + " to your account on " + DateTime.Now;

                await _repo.Notification.AddAsync(notification);
            }

            return RedirectToAction("Wallet");
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int amount , int accountNo)
        {
            var username = User.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            var model = await _repo.AppUser.GetUserLandingPageData(user.Email);
            var bankAccount = model.bankAccount;
            var recieverAcc =( await _repo.BankAccount.GetAllAsync()).Where(x => x.AccountNumber == accountNo.ToString() && x.BankAccountType == "Main").FirstOrDefault();
            
            if (amount > 0)
            {
                if (recieverAcc is not null)
                {
                    recieverAcc.Balance = recieverAcc.Balance + amount;
                    await _repo.BankAccount.UpdateAsync(recieverAcc);
                }

                bankAccount.Balance = bankAccount.Balance - amount;
                await _repo.BankAccount.UpdateAsync(bankAccount);
                Transaction transaction = new Transaction();
                transaction.Amount = amount;
                transaction.UserEmail = user.Email;
                transaction.TransactionDate = DateTime.Now;
                transaction.TrandactionType = "Transfer";
                transaction.BankAccountIdReceiver = accountNo;
                transaction.BankAccountIdSender = int.Parse(bankAccount.AccountNumber);
                await _repo.Transaction.AddAsync(transaction);

                Notification notification = new Notification();
                notification.UserEmail = user.Email;
                notification.NotificationDate = DateTime.Now;
                notification.Message = "You have transerred R" + amount + " to bank account number :" + bankAccount.AccountNumber + " on " + DateTime.Now;

                await _repo.Notification.AddAsync(notification);
            }

            return RedirectToAction("Wallet");
        }



    }

}
