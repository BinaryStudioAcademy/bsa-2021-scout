using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Interfaces.Read;
using Application.Interfaces;

namespace Infrastructure.Mail
{
    public class GmailSmtp : ISmtp
    {
        private readonly SmtpClient _client;
        private readonly MailAddress _from;
        private readonly IMailTemplateReadRepository _repository;
        private readonly IMailBuilderService _mailBuilder;

        public GmailSmtp(IMailTemplateReadRepository repository, IMailBuilderService mailBuilder)
        {
            string address = Environment.GetEnvironmentVariable("GMAIL_ADDRESS");
            string password = Environment.GetEnvironmentVariable("GMAIL_PASSWORD");

            _repository = repository;
            _mailBuilder = mailBuilder;
            _from = new MailAddress(address, "Scout Notifications");

            _client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(address, password),
            };
        }

        public async Task SendAsync(string to, string subject, string body, string templateSlug = "default")
        {
            string html = await _mailBuilder.Build(body, templateSlug);
            MailMessage message = GenerateMessage(to, subject, html);

            _client.Send(message);
        }

        public async Task SendAsync(IEnumerable<string> to, string subject, string body, string templateSlug = "default")
        {
            string html = await _mailBuilder.Build(body, templateSlug);
            MailMessage message = GenerateMessage(to, subject, html);

            _client.Send(message);
        }

        private MailMessage GenerateMessage(string to, string subject, string html)
        {
            MailMessage message = new MailMessage
            {
                From = _from,
                Subject = subject,
                Body = html,
                IsBodyHtml = true,
            };

            message.To.Add(new MailAddress(to));

            return message;
        }

        private MailMessage GenerateMessage(IEnumerable<string> to, string subject, string html)
        {
            MailMessage message = new MailMessage
            {
                From = _from,
                Subject = subject,
                Body = html,
                IsBodyHtml = true,
            };

            foreach (string address in to)
            {
                message.To.Add(new MailAddress(address));
            }

            return message;
        }
    }
}
