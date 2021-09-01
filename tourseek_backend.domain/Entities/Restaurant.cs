using System;
using tourseek_backend.domain.Entities.Base;

namespace tourseek_backend.domain.Entities
{
    public class Restaurant : IBaseIdEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CuisineId { get; set; }
        public Guid SpecialDietId { get; set; }
        public Guid MealId { get; set; }
        public Guid FeatureId { get; set; }
        public Guid LocationId { get; set; }
        public Guid ContactId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Cuisine Cuisine { get; set; }
        public virtual SpecialDiet SpecialDiet { get; set; }
        public virtual Meal Meal { get; set; }
        public virtual Feature Feature { get; set; }
        public virtual Location Location { get; set; }
        public virtual Contact Contact { get; set; }
    }
}