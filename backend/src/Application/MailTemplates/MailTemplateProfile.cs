using AutoMapper;
using Domain.Entities;
using Application.MailTemplates.Dtos;

namespace Application.MailTemplates
{
    public class MailTemplateProfile : Profile
    {
        public MailTemplateProfile()
        {
            CreateMap<MailTemplate, MailTemplateDto>();
        }
    }
}
