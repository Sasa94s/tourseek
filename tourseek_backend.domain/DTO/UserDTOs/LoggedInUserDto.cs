using System.Collections.Generic;
using tourseek_backend.domain.DTO.RoleDTOs;

namespace GlassStoreCore.BL.DTOs.UsersDtos
{
    public class LoggedInUserDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public ICollection<RoleNameDto> Roles { get; set; }
    }
}
