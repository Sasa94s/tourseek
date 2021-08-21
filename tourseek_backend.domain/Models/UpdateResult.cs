using System.Collections.Generic;
using System.Collections.Immutable;

namespace tourseek_backend.domain.Models
{
    public class UpdateResult<TResult, TProxyDto> where TProxyDto : class
    {
        public int AffectedCount { get; set; }
        public ImmutableList<TResult> EntitiesValuesList { get; set; }
        public ICollection<UpdateActionResult<TResult, TProxyDto>> ActionResult { get; set; }
    }
}