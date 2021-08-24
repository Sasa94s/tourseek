using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tourseek_backend.domain.Core;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.repository.UnitOfWork;

namespace tourseek_backend.services.UsersService
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MappingProfile _mapper;

        public UserService(UnitOfWork unit, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, MappingProfile mapper)
        {
            _unit = unit;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AssignUserRole(string userId, RoleNameDto role)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;

            return await _userManager.AddToRoleAsync(slectedUser, role.Name);
        }

        public async Task<IdentityResult> AssignUserRoles(string userId, ICollection<RoleNameDto> roles)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            var rolNames = new List<string>();
            foreach (var role in roles)
            {
                rolNames.Add(role.Name);
            }
            return await _userManager.AddToRolesAsync(slectedUser, rolNames);
        }

        public LoggedInUserDto Authenticate(LoginUserDto userDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> CreateUser(CreateUserDto user)
        {
            var rolesNames = new List<string>();

            var newUser = _mapper.Mapper.Map<CreateUserDto, ApplicationUser>(user);
            newUser.NormalizedEmail = newUser.Email.ToUpper();
            newUser.NormalizedUserName = newUser.UserName.ToUpper();

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                newUser = null;
            }

            foreach (var role in user.Roles)
            {
                rolesNames.Add(role.Name);
            }

            await _userManager.AddToRolesAsync(newUser, rolesNames);
            return newUser;
        }

        public async Task<List<RoleNameDto>> GetUserRoles(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var rolesDtos = new List<RoleNameDto>();
            foreach (var role in roles)
            {
                rolesDtos.Add(new RoleNameDto
                {
                    Name = role
                });
            }
            return rolesDtos;
        }

        public bool SignOut()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UnAssignUserRole(string userId, RoleNameDto role)
        {
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            return await _userManager.RemoveFromRoleAsync(slectedUser, role.Name);
        }

        public async Task<IdentityResult> UnAssignUserRoles(string userId, ICollection<RoleNameDto> roles)
        {
            var rolNames = new List<string>();
            foreach (var role in roles)
            {
                rolNames.Add(role.Name);
            }
            var slectedUser = _userManager.FindByIdAsync(userId).Result;
            return await _userManager.RemoveFromRolesAsync(slectedUser, rolNames);
        }
    }
}
