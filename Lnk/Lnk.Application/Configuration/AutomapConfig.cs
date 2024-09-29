using AutoMapper;
using Lnk.Application.DTOs;
using Lnk.Domain.Entities;

namespace Lnk.Application.Configuration;

public class AutomapConfig : Profile
{
    public AutomapConfig()
    {
        CreateMap<ApplicationUser, AccountDTO>()
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
        .ReverseMap();
    }
}
