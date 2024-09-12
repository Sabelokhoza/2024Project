using _2024FinalYearProject.Data.Interfaces;
using _2024FinalYearProject.Models;
using Microsoft.EntityFrameworkCore;

namespace _2024FinalYearProject.Data
{
  
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<List<Notification>> GetUserMessages(string email)
        {
            var notifications = await _context.Notifications.Where(x => x.UserEmail == email )
                .OrderByDescending(o => o.NotificationDate).ToListAsync();

            return notifications;
        }
    }
}
