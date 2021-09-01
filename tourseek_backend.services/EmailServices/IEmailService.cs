using System.Threading.Tasks;

namespace tourseek_backend.services.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message);
    }
}
