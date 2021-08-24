using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using tourseek_backend.domain.Entities;

namespace tourseek_backend.services.UsersService
{
    public interface IUserService
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
        public LoggedInUserDto Authenticate(LoginUserDto userDto);
        public Task<List<RoleNameDto>> GetUserRoles(ApplicationUser user);
        public Task<IdentityResult> AssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> AssignUserRoles(string userId, ICollection<RoleNameDto> roles);
        public Task<IdentityResult> UnAssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> UnAssignUserRoles(string userId, ICollection<RoleNameDto> roles);

        public bool SignOut();
    }
}
