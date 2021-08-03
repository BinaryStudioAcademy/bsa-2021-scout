using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Interfaces.Read;
using Application.Interfaces;

namespace Infrastructure.Mail
{
    public class GmailSmtp : ISmtp, IDisposable
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

        public void Dispose()
        {
            _client.Dispose();
        }

        public async Task<string> SendAsync(string to, string subject, string body, string templateSlug = "default")
        {
            return await SendAsync(new string[] { to }, subject, body, templateSlug);
        }

        public async Task<string> SendAsync(IEnumerable<string> to, string subject, string body, string templateSlug = "default")
        {
            string html = await _mailBuilder.Build(body, templateSlug);
            string id = GenerateMessageId();

            MailMessage message = GenerateMessage(to, subject, html, id);

            _client.Send(message);
            return id;
        }

        private MailMessage GenerateMessage(IEnumerable<string> to, string subject, string html, string id)
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

            message.Headers.Add("Message-ID", $"<{id}>");

            return message;
        }

        private string GenerateMessageId()
        {
            return $"{Guid.NewGuid().ToString()}@scout.com";
        }
    }
}
