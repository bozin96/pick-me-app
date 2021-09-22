using AutoMapper;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Models.NotificationDtos;
using PickMeApp.Application.Models.UserDtos;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PickMeApp.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(
                    dest => dest.UserPhoto,
                    opt => opt.MapFrom(src => ImageHelper.ImageToBase64(src.UserImage)));

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<Notification, NotificationDto>()
                .ForMember(
                    dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(
                    dest => dest.UserFromImage,
                    opt => opt.MapFrom(src => src.UserFrom != null ? Convert.ToBase64String(src.UserFrom.UserPhoto) : ""))
                .ForMember(
                    dest => dest.UserFromName,
                    opt => opt.MapFrom(src => src.UserFrom != null ? $"{src.UserFrom.FirstName} {src.UserFrom.LastName}" : ""));
        }
    }
}
