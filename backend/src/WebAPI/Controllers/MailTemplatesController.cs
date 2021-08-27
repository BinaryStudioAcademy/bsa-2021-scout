using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;
using Microsoft.AspNetCore.Authorization;
using Application.Common.Commands;
using Domain.Entities;
using Application.MailTemplates.Commands;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Application.MailAttachments.Dtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MailTemplatesController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<MailTemplateDto>> GetMailTempaleList()
        {
            var query = new GetEntityListQuery<MailTemplateDto>();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MailTemplateDto>> GetMailTempale(string id)
        {
            var query = new GetEntityByIdQuery<MailTemplateDto>(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<MailTemplateDto>> CreateMailTempale([FromBody] MailTemplateCreateDto mailTemplateCreateDto)
        {
            //MailTemplateCreateDto mailTemplateCreateDto = new MailTemplateCreateDto() { 
            //    Html ="sdadasd",
            //    Slug = "asdasd",
            //    Subject ="dsdsd",
            //    MailAttachments = new List<MailAttachmentCreateDto>()
            //    {
            //        new MailAttachmentCreateDto()
            //        {
            //            MailTemplateId = "dasda",
            //            File = File,
            //            Name = File.FileName
            //        }
            //    }
            //};
            var query = new CreateMailTemplateCommand(mailTemplateCreateDto);
            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<ActionResult<MailTemplateDto>> UpdateMailTempale([FromBody] MailTemplateUpdateDto mailTemplateUpdateDto)
        {
            var query = new UpdateMailTemplateCommand(mailTemplateUpdateDto);
            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMailTempale(string id)
        {
            var query = new DeleteMailTemplateCommand(id);
            return Ok(await Mediator.Send(query));
        }
    }
}
