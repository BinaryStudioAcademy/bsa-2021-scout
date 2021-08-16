using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Queries;
using Application.MailTemplates.Dtos;

namespace WebAPI.Controllers
{
    public class MailTemplatesController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<MailTemplateDto>> GetList()
        {
            var query = new GetEntityListQuery<MailTemplateDto>();
            return Ok(await Mediator.Send(query));
        }
    }
}
