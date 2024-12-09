using DATN_Core.DTO;
using DATN_Core.Interface;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
namespace DATN_Infrastructure.Repository
{
    public class EmailReponsitory : IEmail
    {
        private readonly EmailDTO _emailDTO;
        public EmailReponsitory(IOptions<EmailDTO> emailDTO) 
        {
            _emailDTO = emailDTO.Value;
        }
        public async Task SendEmail(string email, string subject, string htmlContent)
        {
            var mess = new MimeMessage();
            mess.From.Add(new MailboxAddress("Lạnh quá đi", _emailDTO.Username ?? throw new ArgumentNullException(nameof(_emailDTO.Username))));

            mess.To.Add(new MailboxAddress("", email));
            mess.Subject = subject;

            var bodyEmail = new BodyBuilder();
            bodyEmail.HtmlBody = htmlContent;
            mess.Body = bodyEmail.ToMessageBody();
            using(var ms = new SmtpClient())
            {
                await ms.ConnectAsync(_emailDTO.SmtpServer, _emailDTO.SmtpPort, SecureSocketOptions.StartTls);
                await ms.AuthenticateAsync(_emailDTO.Username, _emailDTO.Password);
                await ms.SendAsync(mess);
                await ms.DisconnectAsync(true);
            }
        }
        public async Task SendEmailAsync(string to, string subject, string htmlContent, byte[] qrCodeImage, string qrCodeFileName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", _emailDTO.Username));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = htmlContent;

            bodyBuilder.LinkedResources.Add(qrCodeFileName, qrCodeImage);

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailDTO.SmtpServer, _emailDTO.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailDTO.Username, _emailDTO.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
