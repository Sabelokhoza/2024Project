using _2024FinalYearProject.Data.Interfaces;

namespace _2024FinalYearProject.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly AppDbContext _appDbContext;
        
        private IChargesRepository _chargesRepository;
        private ITransactionRepository _Transaction;
        private IReviewRepository _Review;
        private INotificationRepository _Notification;
        private IBankAccountRepository _bankAccount;
        private IUserRepository _appUser;
        private ILoginRepository _logins;
        private IAdviceRepository _advice;
        private IReportRepository _report;
        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IAdviceRepository Advice
        {
            get
            {
                if (_advice == null)
                {
                    _advice = new AdviceRepository(_appDbContext);
                }

                return _advice;
            }
        }

        public IBankAccountRepository BankAccount
        {
            get
            {
                if (_bankAccount == null)
                {
                    _bankAccount = new BankAccountRepository(_appDbContext);
                }

                return _bankAccount;
            }
        }

        public IReportRepository report
        {
            get
            {
                if (_report == null)
                {
                    _report = new ReportRepository(_appDbContext);
                }

                return _report;
            }
        }

        public ILoginRepository Logins
        {
            get
            {
                if (_logins == null)
                {
                    _logins = new LoginRepository(_appDbContext);
                }

                return _logins;
            }
        }

        public ITransactionRepository Transaction
        {
            get
            {
                if (_Transaction == null)
                {
                    _Transaction = new TransactionRepository(_appDbContext);
                }

                return _Transaction;
            }
        }

        public INotificationRepository Notification
        {
            get
            {
                if (_Notification == null)
                {
                    _Notification = new NotificationRepository(_appDbContext);
                }

                return _Notification;
            }
        }

        public IChargesRepository Charges
        {
            get
            {
                if (_chargesRepository == null)
                {
                    _chargesRepository = new ChargesRepository(_appDbContext);
                }

                return _chargesRepository;
            }
        }

        public IReviewRepository Review
        {
            get
            {
                if (_Review == null)
                {
                    _Review = new ReviewRepository(_appDbContext);
                }

                return _Review;
            }
        }

        public IUserRepository AppUser
        {
            get
            {
                if (_appUser == null)
                {
                    _appUser = new UserRepository(_appDbContext);
                }

                return _appUser;
            }
        }

        public void SaveChanges()
        {
            _appDbContext.SaveChanges();
        }
    }
}
