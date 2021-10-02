using Microsoft.AspNetCore.Mvc;

namespace tourseek_backend.api.Queries
{
    public class UserQuery
    {

        [FromQuery(Name = "id")]
        public string Id { get; set; }

        [FromQuery(Name = "name")]
        public string Name { get; set; }


    }
}
