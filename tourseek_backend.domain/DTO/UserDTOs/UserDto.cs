using System;
using System.Collections.Generic;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.DTO.UserDTOs
{
    public class UserDto : IBaseEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public ICollection<string> Roles { get; set; }

    }
}
