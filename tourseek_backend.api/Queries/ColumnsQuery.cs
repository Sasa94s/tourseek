using Microsoft.AspNetCore.Mvc;

namespace tourseek_backend.api.Queries
{
    public class ColumnsQuery
    {
        [FromQuery(Name = "columns")] 
        public string Columns { get; set; }
    }
}