using AutoMapper;
using Domain.Entities;
using Application.MailTemplates.Dtos;
using Application.MailAttachments.Dtos;

namespace Application.MailTemplates
{
    public class MailAttachmentProfile : Profile
    {
        public MailAttachmentProfile()
        {
            CreateMap<MailAttachment, MailAttachmentDto>().ReverseMap(); 
            CreateMap<MailAttachmentCreateDto, MailAttachment>();
            CreateMap<MailAttachmentUpdateDto, MailAttachment>();
            CreateMap<MailAttachmentUpdateDto, MailAttachmentCreateDto>();

        }
    }
}
