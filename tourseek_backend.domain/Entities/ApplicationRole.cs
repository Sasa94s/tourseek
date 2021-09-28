using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class ApplicationRole : IdentityRole, IBaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        [NotMapped]
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
