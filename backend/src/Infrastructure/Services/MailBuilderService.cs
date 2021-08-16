using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Infrastructure.Services
{
    public class MailBuilderService : IMailBuilderService
    {
        private readonly IReadRepository<MailTemplate> _repository;

        public MailBuilderService(IReadRepository<MailTemplate> repository)
        {
            _repository = repository;
        }

        public async Task<string> Build(string body, string templateSlug = "default")
        {
            MailTemplate template;

            try
            {
                template = await _repository.GetByPropertyAsync("Slug", templateSlug);
            }
            catch
            {
                throw new NotFoundException("Can't find mail template");
            }

            string resultHtml = template.Html.Replace("{{BODY}}", body);
            return resultHtml;
        }
    }
}
