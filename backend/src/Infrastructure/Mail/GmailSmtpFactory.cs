using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces.Read;

namespace Infrastructure.Mail
{
    public class GmailSmtpFactory : ISmtpFactory
    {
        private readonly IMailBuilderService _mailBuilder;
        private readonly IVaultReadRepository _vaultReadRepository;
        public GmailSmtpFactory(IMailBuilderService mailBuilder, IVaultReadRepository vaultReadRepository)
        {
            _mailBuilder = mailBuilder;
            _vaultReadRepository = vaultReadRepository;
        }

        public async Task<ISmtp> Connect()
        {
            string address = await _vaultReadRepository.ReadSecretAsync("MAIL_ADDRESS");
            string password = await _vaultReadRepository.ReadSecretAsync("MAIL_PASSWORD");
            string displayName = await _vaultReadRepository.ReadSecretAsync("MAIL_DISPLAY_NAME");

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
