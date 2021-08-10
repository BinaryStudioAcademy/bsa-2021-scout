using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Read;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Infrastructure.Services
{
    public class MailBuilderService : IMailBuilderService
    {
        private readonly IMailTemplateReadRepository _repository;

        public MailBuilderService(IMailTemplateReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Build(string body, string templateSlug = "default")
        {
            MailTemplate template;

            try
            {
                template = await _repository.GetBySlugAsync(templateSlug);
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
