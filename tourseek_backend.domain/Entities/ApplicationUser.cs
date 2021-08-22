using Microsoft.AspNetCore.Identity;
using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class ApplicationUser : IdentityUser, IBaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
