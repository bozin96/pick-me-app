using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models.Notification;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<PagedList<Notification>> ListAsync(ResourceParameters resourceParameters, string userId);
    }
}
