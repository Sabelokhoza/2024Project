using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using _2024FinalYearProject.Models.ViewModels.Admin;
using _2024FinalYearProject.Models.ViewModels.Client;
using _2024FinalYearProject.Models.ViewModels.FinacialAdvisor;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace _2024FinalYearProject.Data
{
    public class UserRepository : RepositoryBase<AppUser>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ClientIndexViewModel> GetUserLandingPageData(string email)
        {
            var savingsAccount = await _context.BankAccounts.FirstOrDefaultAsync(x => x.UserEmail == email && x.BankAccountType == "Savings");
            var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(x => x.UserEmail == email && x.BankAccountType == "Main");
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            var transactions = await _context.Transactions.Where(x => x.BankAccountIdReceiver == int.Parse(bankAccount.AccountNumber )|| x.BankAccountIdSender == int.Parse(bankAccount.AccountNumber)).ToListAsync();
            decimal inAmount = 0;
            decimal outAmount = 0;
            foreach (var item in transactions)
            {
                if (item.BankAccountIdReceiver.ToString() == bankAccount.AccountNumber)
                {
                    inAmount += item.Amount;
                }
                else
                {
                    outAmount += item.Amount;
                }
            }

            var model = new ClientIndexViewModel()
            {
                bankAccount = bankAccount,
                transactions = transactions,
                user = user,
                moneyIn = inAmount,
                moneyOut = outAmount,
                Date = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                SavingAccount = savingsAccount
            };

            return model;
           
        }

        public async Task<List<UserViewModel>> GetAllUsersAndBankAccount()
        {
            return await (from user in _context.Users
                                  join account in _context.BankAccounts
                                  on user.Email equals account.UserEmail
                                  where user.UserRole == "Student" || user.UserRole == "Staff"
                                  select new UserViewModel
                                  {
                                      AppUser = user,
                                      BankAccount = account,

                                  }).ToListAsync();
        }

        public async Task<List<AppUser>> GetStaffMembers()
        {
            return await (from user in _context.Users
                          join userRole in _context.UserRoles
                          on user.Id equals userRole.UserId
                          join role in _context.Roles
                          on userRole.RoleId equals role.Id
                          where role.Name == "Consultant" || role.Name == "Advisor"
                          select user).ToListAsync();
        }


        public async Task<UserLogInsViewModel> GetUserLogins(string email)
        {
            var userLogins = await _context.LoginSessions.Where(x => x.UserEmail == email)
                .OrderByDescending(o => o.TimeStamp).ToListAsync();
            return new UserLogInsViewModel
            {
                LoginSessions = userLogins,
            };

        } 
        public async Task<List<Transaction>> GetUserTransactions(string accountNumber)
        {
            var transactions = await _context.Transactions.Where(x => x.BankAccountIdSender == int.Parse(accountNumber) || x.BankAccountIdReceiver == int.Parse(accountNumber))
                .OrderByDescending(o => o.TransactionDate).ToListAsync();

            return transactions;
        }

        public async Task<List<UserAdvisorViewModel>> GetAllUsersAndBankDetails()
        {
            var result = await (from user in _context.Users
                         join account in _context.BankAccounts
                         on user.Email equals account.UserEmail
                         select new UserAdvisorViewModel
                         {
                             MoneyOut = _context.Transactions.Where(x => x.BankAccountIdSender.ToString() == account.AccountNumber).Sum(x => x.Amount),
                             MoneyIn = _context.Transactions.Where(x => x.BankAccountIdReceiver.ToString() == account.AccountNumber).Sum(x => x.Amount),
                             AppUser = user,
                             BankAccount = account
                         }).ToListAsync();

            return result;
        }

        public Task<AdviceViewModel> GetAdviceInformation(string userId)
        {
            var result = from user in _context.Users
                         join account in _context.BankAccounts
                         on user.Email equals account.UserEmail
                         where user.Id == userId
                         select new AdviceViewModel
                         {
                             MoneyOut = _context.Transactions.Where(x => x.BankAccountIdSender.ToString() == account.AccountNumber).Sum(x => x.Amount),
                             MoneyIn = _context.Transactions.Where(x => x.BankAccountIdReceiver.ToString() == account.AccountNumber).Sum(x => x.Amount),
                             Transactions = _context.Transactions.Where(x => x.BankAccountIdReceiver.ToString() == account.AccountNumber || x.BankAccountIdSender.ToString() == account.AccountNumber).ToList(),
                             Client = user,
                             BankAccount = account
                         };
            return result.FirstOrDefaultAsync();
        }
    }
}
