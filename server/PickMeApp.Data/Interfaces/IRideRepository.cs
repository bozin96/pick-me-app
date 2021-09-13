using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IRideRepository : IGenericRepository<Ride>
    {
        Task<PagedList<Ride>> ListAsync(RideResourceParameters resourceParameters);
    }
}
