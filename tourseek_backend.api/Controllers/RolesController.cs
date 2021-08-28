using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tourseek_backend.domain.Core;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
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
        public RolesController(IUnitOfWork unitOfWork)
        {
            _mapper = new MappingProfile();
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationRole> GetRole(string id)
        {
            var role = _unitOfWork.Repository<ApplicationRole>().GetById(id);

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
            var result = _unitOfWork.Repository<ApplicationRole>().Add(role);

            if (!result)
            {
                return BadRequest(new GetJsonResponse
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
            var role = _unitOfWork.Repository<ApplicationRole>().GetById(id);

            if (role == null)
            {
                return NotFound(new GetJsonResponse
                {
                    StatusMessage = "Selected role not found.",
                    Success = false
                });
            }

            _mapper.Mapper.Map(roleDto, role);

            var result = _unitOfWork.Repository<ApplicationRole>().Update(role);

            if (!result)
            {
                return BadRequest(new OtherJsonResponse
                {
                    StatusMessage = "Couldn't update selected role.",
                    Success = false
                });
            }

            return Ok(new GetJsonResponse
            {
                StatusMessage = "Selected role has been updated.",
                Success = true

            });
        }
    }
}
