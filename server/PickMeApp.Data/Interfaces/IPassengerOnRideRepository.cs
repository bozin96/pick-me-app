using PickMeApp.Application.Helpers;
using PickMeApp.Core.Models;
using System;
using System.Threading.Tasks;

namespace PickMeApp.Application.Interfaces
{
    public interface IPassengerOnRideRepository
    {
        Task<PassengerOnRide> GetByIdAsync(Guid id);
        Task<PagedList<PassengerOnRide>> ListAsync(PassengerOnRideResourceParameters resourceParameters);
        Task<PassengerOnRide> AddAsync(PassengerOnRide entity);
        Task<bool> AddReviewAsync(Guid id, int review);
    }
}
