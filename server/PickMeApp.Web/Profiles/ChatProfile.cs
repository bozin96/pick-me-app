using AutoMapper;
using PickMeApp.Application.Models.ChatDtos;
using PickMeApp.Core.Models.Message;
using System;

namespace PickMeApp.Web.Profiles
{
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatDto>()
                .ForMember(
                    dest => dest.ChatId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FirstUserName,
                    opt => opt.MapFrom(src => src.FirstUser == null ? null : $"{src.FirstUser.FirstName} {src.FirstUser.LastName}"))
                .ForMember(
                    dest => dest.FirstUserPhoto,
                    opt => opt.MapFrom(src => (src.FirstUser != null && src.FirstUser.UserAvatarPhoto != null) ? Convert.ToBase64String(src.FirstUser.UserAvatarPhoto) : ""))
                .ForMember(
                    dest => dest.SecondUserName,
                    opt => opt.MapFrom(src => src.SecondUser == null ? null : $"{src.SecondUser.FirstName} {src.SecondUser.LastName}"))
                .ForMember(
                    dest => dest.SecondUserPhoto,
                    opt => opt.MapFrom(src => (src.SecondUser != null && src.SecondUser.UserAvatarPhoto != null) ? Convert.ToBase64String(src.SecondUser.UserAvatarPhoto) : ""));
        }
    }
}
