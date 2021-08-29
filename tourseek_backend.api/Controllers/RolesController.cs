using Microsoft.AspNetCore.Mvc;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.services.Exceptions;
using tourseek_backend.services.RolesService;
using tourseek_backend.util.JsonResponses;

namespace tourseek_backend.api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationRole> GetRole(string id)
        {
            try
            {
                var data = _roleService.GetRoleById(id);
                return Ok( new GetJsonResponse
                {
                    StatusMessage = "Role has been selected successfully.",
                    Success = true,
                    Data = data
                });
            }
            catch (RoleBaseException e)
            {
                return StatusCode(e.StatusCode, e.Message);
            }
        }

        [HttpPost]
        public ActionResult<ApplicationRole> CreateRole(RoleDto roleDto)
        {
            try
            {
                var data = _roleService.CreateRole(roleDto);
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            try
            {
                var data = _roleService.UpdateRole(roleDto, id);
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationRole> DeleteRole(string id)
        {
            try
            {
                var data = _roleService.DeleteRole(id);
                return Ok( new GetJsonResponse
                {
                    StatusMessage = data,
                    Success = true
                });
            }
            catch (RoleBaseException e)
            {
                return StatusCode(e.StatusCode, e.GetResponseBody());
            }
        }
    }
}
