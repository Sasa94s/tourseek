﻿using System;
using System.Collections.Generic;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class SpecialDiet : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}