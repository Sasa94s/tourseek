﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tourseek_backend.domain.DTO.RoleDTOs;
using tourseek_backend.domain.Entities;
using tourseek_backend.domain.Models;
using tourseek_backend.domain.Models.Filters;
using tourseek_backend.repository.GenericRepository;
using tourseek_backend.repository.UnitOfWork;
using tourseek_backend.services.Exceptions;

namespace tourseek_backend.services.RolesService
{
    public class RoleService : BaseService<ApplicationRole, RoleDto, RoleFilter>, IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ApplicationRole> _repository;
        private readonly IGenericRepository<ApplicationUserRole> _userRoleRepository;

        public RoleService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mapper = mapper;
            _repository = unitOfWork.Repository<ApplicationRole>();
            _userRoleRepository = unitOfWork.Repository<ApplicationUserRole>();
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
            role.NormalizedName = roleDto.Name.ToUpper();
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

        public PagedList<dynamic> GetPagedList(string[] getColumns, RoleFilter filter, PaginationFilter paginationFilter)
        {
            return base.GetPagedList(getColumns, filter, paginationFilter);
        }

        public override IQueryable<RoleDto> QuerySelector(
            DbSet<ApplicationRole> entities,
            IQueryable<ApplicationRole> queryable
        )
        {
            return queryable.Select(s => new RoleDto
            {
                Id = s.Id,
                Name = s.Name
            });
        }
    }
}