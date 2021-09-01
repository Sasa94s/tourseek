using System;
using System.Collections.Generic;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class StoryPoint : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid HotelId { get; set; }
        public Guid AssignedServiceId { get; set; }
        public Guid RestaurantId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Hotel Hotel { get; set; }
        public virtual AssignedService AssignedService { get; set; }
        public virtual Restaurant Restaurant { get; set; }

        public virtual TourProgram TourProgram { get; set; }
    }
}