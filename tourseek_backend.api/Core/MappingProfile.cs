using AutoMapper;
using tourseek_backend.api.Queries;
using tourseek_backend.domain.DTO;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.DTO.UserRoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.domain.Models.Responses;

namespace tourseek_backend.api.Core
{
    public class MappingProfile : Profile
    {
        public IMapper Mapper { get; set; }
        public MappingProfile()
        {
            CreateMap<ApplicationUser, CreateUserDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUserRole, UserRoleDto>().ReverseMap();
            CreateMap<RoleDto, ApplicationRole>().ReverseMap();
            CreateMap<UserRoleDto, ApplicationUserRole>().ReverseMap();
            CreateMap<UpdateRoleDto, ApplicationRole>().ReverseMap();
            CreateMap<ApplicationUserRole, UpdateUserRole>().ReverseMap();
            CreateMap<RoleFilter, RoleQuery>().ReverseMap();
            CreateMap<PaginationFilter, PaginationQuery>().ReverseMap();
            CreateMap<PageOutputMeta, PagedResponse<object>>().ReverseMap();

        }
    }
}