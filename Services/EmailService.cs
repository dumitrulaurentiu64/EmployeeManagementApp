using EmpAPI.Dtos;
using EmpAPI.Models;
using EmpAPI.Repository;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmpAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IAuthRepository _authRepository;
        public EmailService(EmailConfiguration emailConfig, IAuthRepository authRepository)
        {
            _emailConfig = emailConfig;
            _authRepository = authRepository;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange((IEnumerable<InternetAddress>)message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        public void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public void CreateAccount(string email, string firstname, int parentId, string role)
        {
            // Create Random Password
            string password = RandomString(10);

            // Create Account
            RegisterDto registerDto = new RegisterDto
            {
                Email = email,
                Firstname = firstname,
                Password = password,
                Role = role
            };
            _authRepository.Register(registerDto, parentId);

            // Send Email
            var message = new Message(new string[] { email }, "Account creation",
                "Hello " + firstname + ",\n \n" +
                "Your account was succesfully created. \n " +
                "You can now login with the following credentials: \n" +
                "Email:" + registerDto.Email + "\n" +
                "Password:" + registerDto.Password + "\n" +
                "You can change your password via profile page.");
            SendEmail(message);
        }

        public void UpdateAccount(int parentId, string role)
        {
            _authRepository.UpdateRole(parentId, role);
        }

        public string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}


