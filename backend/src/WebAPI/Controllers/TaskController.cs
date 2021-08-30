using Application.Tasks.Commands;
using Application.Common.Commands;
using Application.Common.Queries;
using Application.Tasks.Dtos;
using Application.Tasks.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Services;
using Application.Interfaces;
using Application.Users.Dtos;
using Application.Tasks.Dtos;

namespace WebAPI.Controllers
{
    public class TaskController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAsync(string id)
        {
            var query = new GetTaskWithTeamMembersByIdQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskAsync()
        {
            var query = new GetTasksWithTeamMembersQuery();

            return Ok(await Mediator.Send(query));
        }
        
        [HttpPost]
        public async Task<IActionResult> PostTaskAsync([FromBody] CreateTaskDto createDto)
        {
                        
            var query = new CreateTaskCommand(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutTasktAsync([FromBody] UpdateTaskDto updateDto)
        {
            var query = new UpdateTaskCommand(updateDto);

            return Ok(await Mediator.Send(query));
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTaskAsync(string id)
        {
            var query = new DeleteTaskCommand(id);
            await Mediator.Send(query);
            
            return NoContent();

        }        
    }
}