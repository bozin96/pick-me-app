using AutoMapper;
using PickMeApp.Application.Helpers;
using PickMeApp.Application.Models.NotificationDtos;
using PickMeApp.Application.Models.UserDtos;
using PickMeApp.Core.Models;
using PickMeApp.Core.Models.Notification;
using System;

namespace PickMeApp.Web.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForMember(
                    dest => dest.UserPhoto,
                    opt => opt.MapFrom(src => ImageHelper.ImageFromBase64(src.UserPhoto)));

            CreateMap<ApplicationUser, UserDto>()
                .ForMember(
                    dest => dest.UserPhoto,
                    opt => opt.MapFrom(src => src.UserPhoto == null ? null : Convert.ToBase64String(src.UserPhoto)));

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
