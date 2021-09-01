using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class AssignedService : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid ServiceProviderId { get; set; }
        public decimal Fee { get; set; }
        public decimal Tax { get; set; }
        public Guid LocationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public Service Service { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public Location Location { get; set; }
    }
}