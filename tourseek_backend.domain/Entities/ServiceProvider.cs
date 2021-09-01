using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class ServiceProvider : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Guid JobId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string PhoneNumber2 { get; set; }
        public Guid LocationId1 { get; set; }
        public Guid LocationId2 { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Job Job { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Location Address1 { get; set; }
        public virtual Location Address2 { get; set; }
    }
}