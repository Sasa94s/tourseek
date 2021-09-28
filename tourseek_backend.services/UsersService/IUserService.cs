using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;

namespace tourseek_backend.services.UsersService
{
    public interface IUserService
    {
        public Task<ApplicationUser> CreateUser(CreateUserDto user);
        public LoggedInUserDto Authenticate(LoginUserDto userDto);
        public LoggedInUserDto AuthenticateUsingPhone(LoginUserDtoPhone userDto);
        public Task<IdentityResult> AssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> AssignUserRoles(string userId, ICollection<RoleNameDto> roles);
        public Task<IdentityResult> UnAssignUserRole(string userId, RoleNameDto role);
        public Task<IdentityResult> UnAssignUserRoles(string userId, ICollection<RoleNameDto> roles);
        public Task<bool> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        PagedList<dynamic> GetPagedList(string[] getColumns, UserFilter filter, PaginationFilter paginationFilter);
        public Task<List<string>> GetUserRoles(string userId);

        public bool SignOut();
    }
}
