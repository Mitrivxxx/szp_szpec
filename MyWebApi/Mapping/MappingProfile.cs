using AutoMapper;
using MyWebApi.DTOs;
using MyWebApi.Model;

namespace MyWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            {
                CreateMap<User, UserDto>();
                CreateMap<CreateUserDto, User>()
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
                CreateMap<UpdateUserDto, User>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }
        }
    }
}
