using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.JwtAuth;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.util;
using AutoMapper;

namespace tourseek_backend.services.UsersService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unit, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper)
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
            string password = userDto.Password;

            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = _unit.Repository<ApplicationUser>().Get(x => x.UserName == userDto.UserName)
                .SingleOrDefault();

            var roles = _userManager.GetRolesAsync(user);
            ICollection<RoleNameDto> roleNames = new List<RoleNameDto>();
            foreach (var role in roles.Result)
            {
                roleNames.Add(new RoleNameDto
                {
                    Name = role
                });
            }


            // check if username exists
            if (user == null)
                return null;

            var signInResult = _signInManager.PasswordSignInAsync(user, password, true, false);

            // check if password is correct
            if (!signInResult.Result.Succeeded)
                return null;

            // authentication successful
            return new LoggedInUserDto
            {
                UserID = user.Id,
                UserName = user.UserName,
                Roles = roleNames
            };
        }


        public LoggedInUserDto AuthenticateUsingPhone(LoginUserDtoPhone userDto)
        {
            string password = userDto.Password;

            if (string.IsNullOrEmpty(userDto.Phone) || string.IsNullOrEmpty(userDto.Password))
                return null;

            var user = _unit.Repository<ApplicationUser>().Get(x => x.PhoneNumber == userDto.Phone)
                .SingleOrDefault();



            // check if username exists
            if (user == null)
                return null;

            var roles = _userManager.GetRolesAsync(user);
            ICollection<RoleNameDto> roleNames = new List<RoleNameDto>();
            foreach (var role in roles.Result)
            {
                roleNames.Add(new RoleNameDto
                {
                    Name = role
                });
            }

            var signInResult = _signInManager.PasswordSignInAsync(user, password, true, false);

            // check if password is correct
            if (!signInResult.Result.Succeeded)
                return null;

            // authentication successful
            return new LoggedInUserDto
            {
                UserID = user.Id,
                UserName = user.UserName,
                Roles = roleNames
            };
        }

        public async Task<ApplicationUser> CreateUser(CreateUserDto user)
        {
            var rolesNames = new List<string>();

            var newUser = _mapper.Map<CreateUserDto, ApplicationUser>(user);
            newUser.NormalizedEmail = newUser.Email.ToUpper();
            newUser.NormalizedUserName = newUser.UserName.ToUpper();

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                return null;
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
            _signInManager.SignOutAsync();
            JwtToken.RemoveCurrnetToken();
            Singleton singleton = Singleton.GetInstance;

            if (singleton.JwtToken == string.Empty)
                return true;
            else
                return false;
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
