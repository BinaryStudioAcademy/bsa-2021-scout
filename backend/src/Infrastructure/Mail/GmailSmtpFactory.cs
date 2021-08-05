using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Interfaces.Read;
using Application.Interfaces;

namespace Infrastructure.Mail
{
    public class GmailSmtpFactory : ISmtpFactory
    {
        private readonly IMailTemplateReadRepository _repository;
        private readonly IMailBuilderService _mailBuilder;

        public GmailSmtpFactory(
            IMailTemplateReadRepository repository,
            IMailBuilderService mailBuilder
        )
        {
            _repository = repository;
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
                _repository,
                _mailBuilder
            );
        }
    }
}
