using AutoMapper;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.DTO.UserRoleDTOs;
using tourseek_backend.domain.Entities;

namespace tourseek_backend.domain.Core
{
    public class MappingProfile : Profile
    {
        public IMapper Mapper { get; set; }
        public MappingProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
                cfg.CreateMap<ApplicationUser, UserDto>().ReverseMap();
                cfg.CreateMap<ApplicationUserRole, UserRoleDto>().ReverseMap();
                cfg.CreateMap<RoleDto, ApplicationRole>().ReverseMap();
                cfg.CreateMap<UserRoleDto, ApplicationUserRole>().ReverseMap();
                cfg.CreateMap<UpdateRoleDto, ApplicationRole>().ReverseMap();
                cfg.CreateMap<ApplicationUserRole, UpdateUserRole>().ReverseMap();
            });

            this.Mapper = config.CreateMapper();

        }
    }
}