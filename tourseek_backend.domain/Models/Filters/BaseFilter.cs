using System;

namespace tourseek_backend.domain.Models.Filters
{
    public class BaseFilter : IBaseFilter
    {
        public Guid? ExactId { get; set; }
        public bool HasExactId => ExactId != null;
    }
}