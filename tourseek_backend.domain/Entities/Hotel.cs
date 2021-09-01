using System;
using System.Collections.Generic;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class Hotel : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Guid> HotelServiceIds { get; set; }
        public int Stars { get; set; }
        public decimal RatePerNight { get; set; }
        public Guid LocationId { get; set; }
        public Guid ContactId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<HotelService> HotelServices { get; set; }
        public virtual Location Location { get; set; }
        public virtual Contact Contact { get; set; }
    }
}