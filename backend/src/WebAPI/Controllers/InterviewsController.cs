using Application.Interviews.Commands.Create;
using Application.Interviews.Dtos;
using Application.Interviews.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class InterviewsController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetInterviewList()
        {
            var query = new GetInterviewsListQuery();
            return Ok(await Mediator.Send(query));
        }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> GetInterview(string id)
        // {
        //     var query = new GetInterviewsByIdQuery(id);
        //     return Ok(await Mediator.Send(query));
        // }

        [HttpPost]
        public async Task<IActionResult> CreateInterview(CreateInterviewDto interview)
        {
            var command = new CreateInterviewCommand(interview);
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInterview(UpdateInterviewDto interview)
        {
            var command = new UpdateInterviewCommand(interview);
            return StatusCode(201, await Mediator.Send(command));
        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteInterview(string id)
        // {
        //     var command = new DeleteInterviewCommand(id);
        //     return StatusCode(204, await Mediator.Send(command));
        // }
    }
}
