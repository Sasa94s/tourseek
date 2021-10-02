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
using System;
using Microsoft.Extensions.Configuration;
using System.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using tourseek_backend.services.EmailServices;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.domain.Models;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.services.RolesService;

namespace tourseek_backend.services.UsersService
{
    public class UserService : BaseService<ApplicationUser, UserDto, UserFilter>, IUserService
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailSender;

        public UserService(IUnitOfWork unit, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IMapper mapper, IConfiguration configuration, IEmailService emailSender) : base(unit)
        {
            _unit = unit;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _configuration = configuration;
            _emailSender = emailSender;
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

        public async Task<bool> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {

            if (string.IsNullOrEmpty(confirmEmailDto.Id) || string.IsNullOrEmpty(confirmEmailDto.Token))
                return false;

            var code = HttpUtility.UrlEncode(confirmEmailDto.Token);


            var user = await _userManager.FindByIdAsync(confirmEmailDto.Id);
            var result = await _userManager.ConfirmEmailAsync(user, HttpUtility.UrlDecode(confirmEmailDto.Token));

            if (result.Succeeded)
            {
                return true;
            }

            return false;

        }

        public async Task<ApplicationUser> CreateUser(CreateUserDto user)
        {

            ICollection<string> rolesNames = new List<string>();

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
                rolesNames.Add(role);
            }



            await _userManager.AddToRolesAsync(newUser, rolesNames);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["userid"] = newUser.Id;
            query["token"] = token;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();
            var senderEmail = _configuration["ReturnPaths:SenderEmail"];
            await _emailSender.SendEmailAsync(senderEmail, newUser.Email, "Confirm your email address", urlString);

            return newUser;
        }

        public async Task<List<string>> GetUserRoles(string userId)
        {
            var user = _unit.Repository<ApplicationUser>().GetById(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
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

        public PagedList<dynamic> GetPagedList(string[] getColumns, UserFilter filter, PaginationFilter paginationFilter)
        {
            return base.GetPagedList(getColumns, filter, paginationFilter);
        }

        public override IQueryable<UserDto> QuerySelector(
            DbSet<ApplicationUser> entities,
            IQueryable<ApplicationUser> queryable
        )
        {
            var selectedUser = queryable.Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Roles = null
            });

            var users = selectedUser.ToList();
            foreach (var user in users)
            {
                user.Roles = GetUserRoles(user.Id).Result;
            }

            return users.AsQueryable();
        }
    }
}
