using Application.Projects.CommandQuery.Update;
using Application.Projects.Commands.Create;
using Application.Projects.Dtos;
using Application.Projects.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetProjectList()
        {
            var query = new GetProjectListQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(string id)
        {
            var query = new GetProjectByIdQuery(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(ProjectDto project)
        {
            var command = new CreateProjectCommand(project);
            return StatusCode(201, await Mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(ProjectDto project)
        {
            var command = new UpdateProjectCommand(project);
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
