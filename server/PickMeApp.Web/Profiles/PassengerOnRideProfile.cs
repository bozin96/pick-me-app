using AutoMapper;
using PickMeApp.Application.Models.PassengerOnRideDtos;
using PickMeApp.Core.Models;

namespace PickMeApp.Web.Profiles
{
    public class PassengerOnRideProfile : Profile
    {
        public PassengerOnRideProfile()
        {
            CreateMap<PassengerOnRide, PassengerOnRideDto>();
        }
    }
}
