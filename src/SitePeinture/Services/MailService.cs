using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SitePeinture.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace SitePeinture.Services
{
    public class MailService
    {
        private string _smtpAddress;
        private string _smtpFrom;
        private string _smtpPassword;
        private int _smtpPort;
        private string _smtpTo;
        private string _smtpUser;

        public MailService(IConfiguration configuration)
        {
            this._smtpAddress = configuration.GetSection("mail:smtp:address").Value;
            this._smtpPort = int.Parse(configuration.GetSection("mail:smtp:port").Value);
            this._smtpUser = configuration.GetSection("mail:smtp:user").Value;
            this._smtpPassword = configuration.GetSection("mail:smtp:password").Value;
            this._smtpTo = configuration.GetSection("mail:to").Value;
            this._smtpFrom = configuration.GetSection("mail:from").Value;
        }


        internal void SendMail(Contact contact)
        {
            MimeMessage message = new MimeMessage();
            message.Subject = $"Contact depuis le formulaire du site";
            message.Body = new TextPart { Text = $"Message envoyé par [{contact.Name}]\r\n mail saisi [{contact.Mail}] \r\n Message : \r\n {contact.Text}" };
            message.From.Add(new MailboxAddress("contact", this._smtpFrom));
            message.To.Add(new MailboxAddress("contact", this._smtpTo));

            using (var client = new SmtpClient())
            {
                client.Timeout = 3000;
                client.Connect(this._smtpAddress, this._smtpPort, SecureSocketOptions.SslOnConnect);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(this._smtpUser, this._smtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
