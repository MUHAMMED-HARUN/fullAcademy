
using AutoMapper;
using System.Application.Services.Authentication.AuthDTO;
using System.Application.Services.Authentication.Login.DTO;

namespace System.Application.Services.Authentication.Login.Mapper
{
    public class LoginProfile: Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginDto,AuthDTOInfo>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => ParseGuid(src.ID)))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));
        }
        private static Guid ParseGuid(string id)
        {
            return Guid.TryParse(id, out Guid result) ? result : Guid.Empty;
        }
    }
}
