using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class Service : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}