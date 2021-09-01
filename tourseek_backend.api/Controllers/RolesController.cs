using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using tourseek_backend.api.Helpers;
using tourseek_backend.api.Queries;
using tourseek_backend.domain.DTO;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.domain.Models.Responses;
using tourseek_backend.services;
using tourseek_backend.services.Exceptions;
using tourseek_backend.services.RolesService;
using tourseek_backend.util.JsonResponses;

namespace tourseek_backend.api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleService _roleService;

        public RolesController(IMapper mapper, ILogger<RolesController> logger, IRoleService roleService)
        {
            _mapper = mapper;
            _logger = logger;
            _roleService = roleService;
        }
        
        [HttpGet]
        [Produces(typeof(PagedResponse<dynamic>))]
        public IActionResult GetAll(
            [FromQuery] RoleQuery query, 
            [FromQuery] PaginationQuery paginationQuery,
            [FromQuery] ColumnsQuery columnsQuery
        )
        {
            var logger = new LoggerService<RolesController>(_logger, "Roles", "GetAll");
            PagedResponse<object> response;
            try
            {
                logger.LogRequest(new Tuple<string, object>("Query", query));
                var filter = _mapper.Map<RoleFilter>(query);
                var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
                var pagedData = _roleService.GetPagedList(columnsQuery.GetColumns(), filter, paginationFilter);

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

        [HttpGet("{id}")]
        public ActionResult<ApplicationRole> GetRole(string id)
        {
            var logger = new LoggerService<RolesController>(_logger, "Roles", "GetRole");
            try
            {
                logger.LogRequest(new Tuple<string, object>("ID", id));
                var data = _roleService.GetRoleById(id);
                logger.LogResponse(new Tuple<string, object>("Data", data));
                return Ok( new GetJsonResponse
                {
                    StatusMessage = "Role has been selected successfully.",
                    Success = true,
                    Data = data
                });
            }
            catch (RoleBaseException e)
            {
                logger.LogError(e);
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ApplicationRole> CreateRole(RoleDto roleDto)
        {
            var logger = new LoggerService<RolesController>(_logger, "Roles", "CreateRole");
            try
            {
                logger.LogRequest(new Tuple<string, object>("Role", roleDto));
                var data = _roleService.CreateRole(roleDto);
                logger.LogResponse(new Tuple<string, object>("Message", data));
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                logger.LogError(e);
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var logger = new LoggerService<RolesController>(_logger, "Roles", "UpdateRole");
            try
            {
                logger.LogRequest(new Tuple<string, object>("Role", roleDto), 
                    new Tuple<string, object>("ID", id));
                var data = _roleService.UpdateRole(roleDto, id);
                logger.LogResponse(new Tuple<string, object>("Message", data));
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                logger.LogError(e);
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationRole> DeleteRole(string id)
        {
            var logger = new LoggerService<RolesController>(_logger, "Roles", "DeleteRole");
            try
            {
                logger.LogRequest(new Tuple<string, object>("ID", id));
                var data = _roleService.DeleteRole(id);
                logger.LogResponse(new Tuple<string, object>("Message", data));
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                logger.LogError(e);
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }
    }
}
