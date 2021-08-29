using tourseek_backend.domain.DTO.RoleDTOs;

namespace tourseek_backend.services.RolesService
{
    public interface IRoleService
    {
        RoleDto GetRoleById(string id);
        string CreateRole(RoleDto roleDto);
        string UpdateRole(UpdateRoleDto roleDto, string id);
        string DeleteRole(string id);
    }
}