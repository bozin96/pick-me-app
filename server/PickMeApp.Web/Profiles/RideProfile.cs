using AutoMapper;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Profiles
{
    public class RideProfile : Profile
    {
        public RideProfile()
        {
            CreateMap<Ride, RideDto>();

            CreateMap<RideForCreationDto, Ride>();

            CreateMap<RideForUpdateDto, Ride>();

            CreateMap<Ride, RideForUpdateDto>();
        }

    }
}
