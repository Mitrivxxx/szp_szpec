using AutoMapper;
using MyWebApi.DTOs.Project;
using MyWebApi.DTOs.Users;
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

                CreateMap<Project, ProjectDto>()
                    .ForMember(dest => dest.Users, opt =>
                        opt.MapFrom(src => src.ProjectUsers.Select(pu => pu.User)));

                CreateMap<User, ProjectDto.UserInProjectDto>();
                CreateMap<ProjectDto, Project>();
            }
        }
    }
}
