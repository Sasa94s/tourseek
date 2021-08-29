using System.Net;

namespace tourseek_backend.services.Exceptions
{
    public class RoleNotFoundException : RoleBaseException
    {
        public RoleNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
    }
}