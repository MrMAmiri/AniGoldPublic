using MimeKit;
using System;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using AniGoldShop.Domain.Interfaces.Configuration;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using NETCore.MailKit.Core;

namespace AniGoldShop.Infrastructure.EmailService.MimeKitEmailService
{
    public class MimeKitEmail : Domain.Interfaces.ExternalServices.IEmailService, IDisposable
    {
        private MailboxAddress _receptorEmailAddress;
        private MimeMessage _mimeMessage;
        
        private SmtpClient _smtpClient;

        private IEmailSmtpConfiguration _emailConfiguration;

        public MimeKitEmail(IEmailSmtpConfiguration emailConfiguration)
        {
            // create a mimeMessage
            _mimeMessage = new MimeMessage();

            // set email smtp configuration
            _emailConfiguration = emailConfiguration;

            // Connect to smtp server
            _smtpClient = new SmtpClient();
            _smtpClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort);
        }


        public async Task SendHtmlMessage(string subject, string HtmlMessage)
        {
            _mimeMessage.Subject = subject;
            MimeKit.BodyBuilder bodyBuilder = new MimeKit.BodyBuilder();
            bodyBuilder.HtmlBody = HtmlMessage;
            _mimeMessage.Body = bodyBuilder.ToMessageBody();

            await _sendEmailMessage(_mimeMessage);
        }

        public async Task SendTextMessage(string subject, string messgae)
        {
            _mimeMessage.Subject = subject;

            MimeKit.BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = messgae;
            _mimeMessage.Body = bodyBuilder.ToMessageBody();

            await _sendEmailMessage(_mimeMessage);
        }

        private async Task _sendEmailMessage(MimeMessage mimeMessage)
        {
            // login into smtp server by username and password

            _smtpClient.Authenticate(_emailConfiguration.SmtpUserName, _emailConfiguration.SmtpPassword);
            await _smtpClient.SendAsync(mimeMessage);
        }

        public void SetReceptorEmailAddress(string name, string emailAddress)
        {
            _receptorEmailAddress = new MailboxAddress(name, emailAddress);
            _mimeMessage.To.Add(_receptorEmailAddress);
        }

        public void SetSenderEmailAddress(string name, string emailAddress)
        {
            _receptorEmailAddress = new MailboxAddress(name, emailAddress);
            _mimeMessage.From.Add(_receptorEmailAddress);
        }

        public void Dispose()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }
}
