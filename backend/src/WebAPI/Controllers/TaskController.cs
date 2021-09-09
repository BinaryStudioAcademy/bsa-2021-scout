using Application.Common.Queries;
using Application.Tasks.Commands;
using Application.Tasks.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class TaskController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAsync(string id)
        {
            var query = new GetTaskWithTeamMembersByIdQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTaskbyUserAsync(string userId)
        {
            var query = new GetTasksWithTeamMembersByUserQuery(userId);

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
            createDto.IsReviewed = true;

            var query = new CreateTaskCommand(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutTasktAsync([FromBody] UpdateTaskDto updateDto)
        {
            updateDto.IsReviewed = true;

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