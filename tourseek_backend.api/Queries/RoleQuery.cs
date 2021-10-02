using Microsoft.AspNetCore.Mvc;

namespace tourseek_backend.api.Queries
{
    public class RoleQuery
    {
        [FromQuery(Name = "id")] 
        public string Id { get; set; }
        
        [FromQuery(Name = "name")] 
        public string Name { get; set; }
    }
}