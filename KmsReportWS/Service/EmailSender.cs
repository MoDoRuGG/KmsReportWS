using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;

namespace KmsReportWS.Service
{
    public class EmailSender
    {
        private const string Email = "admin.kmsreport@kapmed.ru";
        private const string NameSender = "KmsReport";
        private const string User = "admin.kmsreport";
        private const string Password = "jwJdWagc4BT3";
        private const string SmtpServer = "smtp.kapmed.ru";
        private const string Sign = "\r\nДанное письмо сформировано автоматически. Пожалуйста, не отвечайте на него.";
        private const int SmtpPort = 587;


        private readonly SmtpClient _smtpClient;

        public EmailSender()
        {
            _smtpClient = new SmtpClient();
        }

        public void Send(List<string> emails, string theme, string body)
        {
            if (!_smtpClient.IsConnected)
                _smtpClient.Connect(SmtpServer, SmtpPort, false);
            if (!_smtpClient.IsAuthenticated)
                _smtpClient.Authenticate(User, Password);
            var emailAddresses = emails.Select(x => x);
            body += Sign;

            var emailMessage = new MimeKit.MimeMessage();

            emailMessage.From.Add(new MailboxAddress(NameSender, User));

            emailMessage.Subject = theme;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = body
            };


            foreach (var email in emailAddresses)
            {
                emailMessage.To.Add(new MailboxAddress(email));
            }

            _smtpClient.Send(emailMessage);
            _smtpClient.Disconnect(true);

        }
    }
}