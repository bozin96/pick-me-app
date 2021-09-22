using iComplyICO.Data;
using Microsoft.EntityFrameworkCore;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Interfaces;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {


        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Notification>> ListAsync(ResourceParameters resourceParameters, string userId)
        {
            if (resourceParameters == null)
            {
                throw new ArgumentNullException(nameof(resourceParameters));
            }

            var collection = _entities as IQueryable<Notification>;

            collection = collection.Where(e => e.UserToId == userId);
            collection = collection.Include(e => e.UserFrom);

            return await PagedList<Notification>.CreateAsync(collection,
                resourceParameters.PageNumber,
                resourceParameters.PageSize);
        }
    }
}
