using AutoMapper;
using PickMeApp.Application.Models.RideDtos;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
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
            CreateMap<Ride, RideDto>()
                .ForMember(
                    dest => dest.DriverName,
                    opt => opt.MapFrom(src => src.Driver != null ? $"{src.Driver.FirstName} {src.Driver.LastName}" : ""))
                 .ForMember(
                    dest => dest.DriverRate,
                    opt => opt.MapFrom(src => src.Driver != null ? src.Driver.AverageRate : 0.0f));

            CreateMap<RideForCreationDto, Ride>();

            CreateMap<RideForUpdateDto, Ride>();

            CreateMap<Ride, RideForUpdateDto>();

            CreateMap<RideRequestDto, RideRequestNotificationPayload>()
                .ReverseMap();
        }

    }
}
