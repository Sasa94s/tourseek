using Microsoft.AspNetCore.Mvc;
using tourseek_backend.domain.Core;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.repository.GenericRepository;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.util.JsonResponses;

namespace tourseek_backend.api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly MappingProfile _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<ApplicationRole> _repository;
        public RolesController(IUnitOfWork unitOfWork)
        {
            _mapper = new MappingProfile();
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Repository<ApplicationRole>();
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationRole> GetRole(string id)
        {
            var role = _repository.GetById(id);

            if (role == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "Role has been selected successfully.",
                Success = true,
                Data = _mapper.Mapper.Map<ApplicationRole, RoleDto>(role)
            });

        }

        [HttpPost]
        public ActionResult<ApplicationRole> CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Mapper.Map<RoleDto, ApplicationRole>(roleDto);
            var result = _repository.Add(role);

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't create role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Role has been created successfully.",
                Success = true,
            });
        }

        [HttpPut("{id}")]
        public ActionResult<ApplicationRole> UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var role = _repository.GetById(id);

            if (role == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            _mapper.Mapper.Map(roleDto, role);

            var result = _repository.Update(role);

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected role.",
                    Success = false
                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Selected role has been updated.",
                Success = true
            });
        }

        [HttpDelete("{id}")]
        public ActionResult<ApplicationRole> DeleteRole(string id)
        {
            var selectedRole = _repository.GetById(id);

            if (selectedRole == null)
            {
                return NotFound(new OtherJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            var result = _repository.Remove(selectedRole);
            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't delete selected role.",
                    Success = false

                });
            }

            return Ok(new OtherJsonResponse
            {
                StatusMessage = "Role has been deleted successfully.",
                Success = true
            });
        }
    }
}
