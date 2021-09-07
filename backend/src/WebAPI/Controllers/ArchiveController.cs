using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Application.Archive.Commands;
using Application.Arhive.Dtos;
using Application.Pools.Queries.GetPoolByIdQueryFull;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ArchiveController : ApiController
    {
        [HttpPut("vacancies/{id}")]
        public async Task<IActionResult> PutArchiveVacancy(string id)
        {
            await Mediator.Send(new ArchiveVacancyCommand(id));
            return Ok();
        }

        [HttpPut("vacancies/{id}/closed")]
        public async Task<IActionResult> PutArchiveClosedVacancy(string id)
        {
            await Mediator.Send(new ArchiveVacancyCommand(id, true));
            return Ok();
        }

        [HttpPut("projects/{id}")]
        public async Task<IActionResult> PutArchiveProject(string id)
        {
            await Mediator.Send(new ArchiveProjectCommand(id));
            return Ok();
        }

        [HttpPut("unarchive/vacancies")]
        public async Task<IActionResult> PutUnarchiveVacancy(ArchivedVacancyDto dto)
        {
            await Mediator.Send(new UnarchiveVacancyCommand(dto));
            return Ok();
        }

        [HttpPut("unarchive/projects")]
        public async Task<IActionResult> PutUnarchiveProject(ArchivedProjectDto dto)
        {
            await Mediator.Send(new UnarchiveProjectCommand(dto));
            return Ok();
        }

        [HttpGet("vacancies")]
        public async Task<ActionResult<IEnumerable<ArchivedVacancyDto>>> GetArchivedVacanciesAsync()
        {
            return Ok(await Mediator.Send(new GetArchivedVacanciesQuery()));
        }

        [HttpGet("projects")]
        public async Task<ActionResult<IEnumerable<ArchivedProjectDto>>> GetArchivedProjectsAsync()
        {
            return Ok(await Mediator.Send(new GetArchivedProjectsQuery()));
        }

        [Authorize(Roles = "HrLead")]
        [HttpDelete("vacancies/{id}")]
        public async Task<IActionResult> DeleteVacancy(string id)
        {
            await Mediator.Send(new DeleteArhivedVacancyCommand(id));
            return Ok();
        }

        [Authorize(Roles = "HrLead")]
        [HttpDelete("projects/{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            await Mediator.Send(new DeleteArhivedProjectCommand(id));
            return Ok();
        }
    }
}
