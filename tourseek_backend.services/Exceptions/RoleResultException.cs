using System.Net;

namespace tourseek_backend.services.Exceptions
{
    public class RoleResultException : RoleBaseException
    {
        public RoleResultException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }
    }
}