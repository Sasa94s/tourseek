using System;
using NpgsqlTypes;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class Location : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string HouseNumber { get; set; }
        public string RoadName { get; set; }
        public Guid CityZoneId { get; set; }
        public string PostalCode { get; set; }
        public NpgsqlPoint Point { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        
        public virtual CityZone CityZone { get; set; }
    }
}