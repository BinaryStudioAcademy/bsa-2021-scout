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
            CreateMap<MailTemplateDto, MailTemplate>();
            CreateMap<MailTemplateCreateDto, MailTemplate>();
            CreateMap<MailTemplateUpdateDto, MailTemplate>();
            CreateMap<MailTemplate, MailTemplateSendDto>();
            CreateMap<MailTemplate, MailTemplateTableDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Slug))
                .ForMember(dest => dest.AttachmentsCount, opt => opt.MapFrom(src => src.MailAttachments.Count));
        }
    }
}
