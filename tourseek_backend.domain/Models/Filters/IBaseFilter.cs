using System;

namespace tourseek_backend.domain.Models.Filters
{
    public interface IBaseFilter
    {
        Guid? ExactId { get; set; }
        bool HasExactId => ExactId != null;
    }
}