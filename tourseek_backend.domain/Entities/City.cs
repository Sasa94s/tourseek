using System;
using System.Collections.Generic;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class City : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Country Country { get; set; }
        
        public virtual ICollection<CityZone> CityZones { get; set; }

    }
}