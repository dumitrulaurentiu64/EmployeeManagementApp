using EmpAPI.Models;
using MimeKit;

namespace EmpAPI.Services
{
    public interface IEmailService
    {
        public void SendEmail(Message message);
        public void Send(MimeMessage mailMessage);
        public MimeMessage CreateEmailMessage(Message message);
    }
}
