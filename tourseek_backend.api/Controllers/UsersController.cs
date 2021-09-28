using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tourseek_backend.api.Helpers;
using tourseek_backend.api.Queries;
using tourseek_backend.domain.DTO;
using tourseek_backend.domain.DTO.UserDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.JwtAuth;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.domain.Models.Responses;
using tourseek_backend.domain.Validators;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.services;
using tourseek_backend.services.RolesService;
using tourseek_backend.services.UsersService;
using tourseek_backend.util.JsonResponses;

namespace tourseek_backend.api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly UserValidator _rules;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userService = userService;
            _rules = new UserValidator();
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<ApplicationUser> CreateUser(CreateUserDto userDto)
        {
            var validationResult = _rules.Validate(userDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var result = _userService.CreateUser(userDto);


            if (result.Result == null)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't create user.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "User has been Created Successfully..",
                Success = true,
            });
        }

        [HttpPut]
        public ActionResult<ApplicationUser> UpdateUser(UserDto userDto)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().GetById(userDto.Id);

            if (user == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Find Selected user",
                    Success = false
                });
            }

            _mapper.Map(userDto, user);

            var selectedUserRoles = _unitOfWork.Repository<ApplicationUserRole>().Get(r => r.UserId == userDto.Id)
                .ToList();

            if (selectedUserRoles.Count > 0)
            {
                foreach (var role in selectedUserRoles)
                {
                    _unitOfWork.Repository<ApplicationUserRole>().Remove(new ApplicationUserRole
                    {
                        UserId = userDto.Id,
                        RoleId = role.RoleId
                    });
                }
            }

            var newRoles = new List<ApplicationRole>();
            foreach (var role in userDto.Roles)
            {
                newRoles.Add(_unitOfWork.Repository<ApplicationRole>().Get(r => r.Name == role).SingleOrDefault());
            }


            try
            {
                foreach (var role in newRoles)
                {
                    _unitOfWork.Repository<ApplicationUserRole>().Add(new ApplicationUserRole { UserId = userDto.Id, RoleId = role.Id });
                }
            }
            catch (System.AggregateException e)
            {
                foreach (var role in selectedUserRoles)
                {
                    _unitOfWork.Repository<ApplicationUserRole>().Add(new ApplicationUserRole
                    {
                        UserId = userDto.Id,
                        RoleId = role.RoleId
                    });
                }
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = e.Message,
                    Success = false
                });
            }

            var result = _unitOfWork.Repository<ApplicationUser>().Update(user);

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected user.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Selected user has been updated successfully.",
                Success = true
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationUser> DeleteUser(string id)
        {
            var selectedUser = _unitOfWork.Repository<ApplicationUser>().GetById(id);
            if (selectedUser == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected user not found.",
                    Success = false
                });
            }

            var result = _unitOfWork.Repository<ApplicationUser>().Remove(selectedUser);

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Faild to Delete selected user.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Selected user has been deleted succesfully.",
                Success = true
            });
        }

        [HttpPost]
        public ActionResult<ApplicationUser> Login(LoginUserDto loginUserDto)
        {
            var result = _userService.Authenticate(loginUserDto);
            var ApiToken = JwtToken.GenerateJwtToken(result);

            if (result == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Wrong UserName or password.",
                    Success = false,
                });
            }
            return Ok(new LoginJsonResponse
            {
                StatusMessage = "User has logged in successfully.",
                Success = true,
                Token = ApiToken,
            });
        }

        [HttpPost]
        public ActionResult<ApplicationUser> LoginUsingPhone(LoginUserDtoPhone loginUserDto)
        {
            var result = _userService.AuthenticateUsingPhone(loginUserDto);

            var ApiToken = JwtToken.GenerateJwtToken(result);

            if (result == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Wrong UserName or password.",
                    Success = false,
                });
            }
            return Ok(new LoginJsonResponse
            {
                StatusMessage = "User has logged in successfully.",
                Success = true,
                Token = ApiToken,
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult<ApplicationUser> LogOut()
        {
            var result = _userService.SignOut();
            if (!result)
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Logout.",
                    Success = false
                });
            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Logged out successfully.",
                Success = true
            });
        }

        [HttpPost]
        public ActionResult<ApplicationUser> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var result = _userService.ConfirmEmail(confirmEmailDto).Result;

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't Confirm Emnail",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Email has been confirmed",
                Success = true
            });
        }


        [HttpGet]
        [Produces(typeof(PagedResponse<dynamic>))]
        public IActionResult GetAll(
            [FromQuery] UserQuery query,
            [FromQuery] PaginationQuery paginationQuery,
            [FromQuery] ColumnsQuery columnsQuery
        )
        {
            var logger = new LoggerService<UsersController>(_logger, "Users", "GetAll");
            PagedResponse<object> response;
            try
            {
                logger.LogRequest(new Tuple<string, object>("Query", query));
                var filter = _mapper.Map<UserFilter>(query);
                var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
                var pagedData = _userService.GetPagedList(columnsQuery.GetColumns(), filter, paginationFilter);

                if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
                {
                    response = new PagedResponse<object>(pagedData.Data);
                }
                else
                {
                    response = paginationFilter.CreatePaginatedResponse(pagedData);
                }
                var pageOutputMeta = _mapper.Map<PageOutputMeta>(response);
                logger.LogResponse(new Tuple<string, object>("PageInfo", pageOutputMeta));

            }
            catch (Exception e)
            {
                logger.LogError(e);
                return BadRequest();
            }

            return Ok(response);
        }

    }
}
