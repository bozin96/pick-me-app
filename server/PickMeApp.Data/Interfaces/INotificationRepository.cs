using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models.Notification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<PagedList<Notification>> ListAsync(ResourceParameters resourceParameters, string userId);
    }
}
