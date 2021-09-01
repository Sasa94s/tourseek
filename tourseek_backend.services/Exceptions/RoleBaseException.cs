using System.Net;
using System.Net.Http;

namespace tourseek_backend.services.Exceptions
{
    public class RoleBaseException : HttpRequestException
    {
        public new int StatusCode { get; }

        public RoleBaseException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = (int) statusCode;
        }

        public BaseResult GetResponseBody()
        {
            return new BaseResult
            {
                Status = StatusCode != 200,
                Message = Message
            };
        }
    }
}