using System;
using Application.Interfaces;

namespace Infrastructure.Mail
{
    public class GmailSmtpFactory : ISmtpFactory
    {
        private readonly IMailBuilderService _mailBuilder;

        public GmailSmtpFactory(IMailBuilderService mailBuilder)
        {
            _mailBuilder = mailBuilder;
        }

        public ISmtp Connect()
        {
            string address = Environment.GetEnvironmentVariable("MAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
            string displayName = Environment.GetEnvironmentVariable("MAIL_DISPLAY_NAME");

            return Connect(address, password, displayName);
        }

        public ISmtp Connect(string address, string password, string displayName)
        {
            return new Smtp(
                address,
                password,
                displayName,
                "smtp.gmail.com",
                587,
                true,
                _mailBuilder
            );
        }
    }
}
