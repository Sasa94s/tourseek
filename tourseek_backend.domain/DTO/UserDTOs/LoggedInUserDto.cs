using System.Collections.Generic;
using tourseek_backend.domain.DTO.RoleDTOs;

namespace tourseek_backend.domain.DTO.UserDTOs
{
    public class LoggedInUserDto
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public ICollection<RoleNameDto> Roles { get; set; }
    }
}
