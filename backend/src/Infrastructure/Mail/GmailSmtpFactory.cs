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
