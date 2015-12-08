using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SitePeinture.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

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
            message.Subject = $"Contact de {contact.Name}, {contact.Mail}";
            message.Body = new TextPart { Text = contact.Text };
            message.From.Add(new MailboxAddress("contact", this._smtpFrom));
            message.To.Add(new MailboxAddress("contact", this._smtpTo));

            using (var client = new SmtpClient())
            {
                client.Connect(this._smtpAddress, this._smtpPort, false);

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
