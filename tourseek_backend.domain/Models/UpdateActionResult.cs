using System.Collections.Generic;

namespace tourseek_backend.domain.Models
{
    public class UpdateActionResult<TKey, TData> where TData : class
    {
        public TKey Key { get; set; }
        public TData Data { get; set; }
        public IDictionary<string, string> ValidationErrors { get; set; }
    }
}