using AutoMapper;
using PickMeApp.Application.Models.PassengerOnRideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
