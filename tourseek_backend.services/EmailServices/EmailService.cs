using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;

namespace tourseek_backend.services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string message)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress, subject, message);

            using (var client = new SmtpClient(_configuration["SMTP:Host"], int.Parse(_configuration["SMTP:Port"]))
            {
                Credentials = new NetworkCredential(_configuration["SMTP:Username"], _configuration["SMTP:Password"])
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
