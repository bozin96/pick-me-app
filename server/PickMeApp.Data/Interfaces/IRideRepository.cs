using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IRideRepository : IGenericRepository<Ride>
    {
        Task<PagedList<Ride>> ListAsync(RideResourceParameters resourceParameters, string driverId="");

        Task<PagedList<Ride>> ListAsync(MyRideResourceParameters resourceParameters);
    }
}
