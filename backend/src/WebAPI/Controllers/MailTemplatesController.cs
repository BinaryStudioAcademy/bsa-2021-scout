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
using Application.MailTemplates.Queries;

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
            var query = new GetMailTemplatesListForThisUserQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MailTemplateDto>> GetMailTempale(string id)
        {
            var query = new GetEntityByIdQuery<MailTemplateDto>(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult<MailTemplateDto>> CreateMailTempale([FromForm] string body, List<IFormFile> files)
        {
            var query = new CreateMailTemplateCommand(body, files);
            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<ActionResult<MailTemplateDto>> UpdateMailTempale([FromForm] string body, List<IFormFile> files)
        {
            var query = new UpdateMailTemplateCommand(body, files);
            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMailTempale(string id)
        {
            var query = new DeleteMailTemplateCommand(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost("sendEmail/{id}/{email}")]
        public async Task<ActionResult<MailTemplateDto>> SendEmail(string id, string email, [FromForm] string body)
        {
            var command = new SendEmailWithTemplateCommand(id, email, body);
            
            return Ok(await Mediator.Send(command));
        }
    }
}
