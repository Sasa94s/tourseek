using System.Net;
using AutoMapper;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.repository.GenericRepository;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.services.Exceptions;

namespace tourseek_backend.services.RolesService
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ApplicationRole> _repository;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.Repository<ApplicationRole>();
        }

        public RoleDto GetRoleById(string id)
        {
            var role = _repository.GetById(id);

            if (role == null)
            {
                throw new RoleNotFoundException("Selected role not found.", HttpStatusCode.NotFound);
            }

            return _mapper.Map<ApplicationRole, RoleDto>(role);
        }

        public string CreateRole(RoleDto roleDto)
        {
            var role = _mapper.Map<RoleDto, ApplicationRole>(roleDto);
            var result = _repository.Add(role);
            if (!result)
            {
                throw new RoleResultException("Couldn't create selected role.", HttpStatusCode.BadRequest);
            }

            return "Selected role has been created.";
        }

        public string UpdateRole(UpdateRoleDto roleDto, string id)
        {
            var role = _repository.GetById(id);
            if (role == null)
            {
                throw new RoleNotFoundException("Selected role not found.", HttpStatusCode.NotFound);
            }
            _mapper.Map(roleDto, role);
            var result = _repository.Update(role);
            if (!result)
            {
                throw new RoleResultException("Couldn't update selected role.", HttpStatusCode.BadRequest);
            }

            return "Role has been updated successfully.";
        }

        public string DeleteRole(string id)
        {
            var role = _repository.GetById(id);
            if (role == null)
            {
                throw new RoleNotFoundException("Selected role not found.", HttpStatusCode.NotFound);
            }
            var result = _repository.Remove(role);
            if (!result)
            {
                throw new RoleResultException("Couldn't delete selected role.", HttpStatusCode.BadRequest);
            }

            return "Role has been deleted successfully.";
        }
    }
}
