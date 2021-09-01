using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class Flat : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Area { get; set; }
        public Guid RateTypeId { get; set; }
        public decimal RateAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        
        public virtual RateType RateType { get; set; }
    }
}