using Application.Common.Commands;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class ApplicantsController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicantAsync(string id)
        {
            var query = new GetEntityByIdQuery<ApplicantDto>(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicantsAsync()
        {
            var query = new GetEntityListQuery<ApplicantDto>();

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicantAsync([FromBody] CreateApplicantDto createDto)
        {
            var query = new CreateEntityCommand<CreateApplicantDto>(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutApplicantAsync([FromBody] UpdateApplicantDto updateDto)
        {
            var query = new UpdateEntityCommand<UpdateApplicantDto>(updateDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicantAsync(string id)
        {
            var query = new DeleteEntityCommand(id);

            return Ok(await Mediator.Send(query));
        }
    }
}