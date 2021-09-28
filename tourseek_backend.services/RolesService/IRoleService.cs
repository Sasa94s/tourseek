using System.Collections.Generic;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.repository.GenericRepository;

namespace tourseek_backend.services.RolesService
{
    public interface IRoleService : IQuery<ApplicationRole, RoleDto, RoleFilter>
    {
        RoleDto GetRoleById(string id);
        string CreateRole(RoleDto roleDto);
        string UpdateRole(UpdateRoleDto roleDto, string id);
        string DeleteRole(string id);
        PagedList<dynamic> GetPagedList(string[] getColumns, RoleFilter filter, PaginationFilter paginationFilter);
    }
}