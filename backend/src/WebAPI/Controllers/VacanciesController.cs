using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stages.Commands;
using Application.Stages.Dtos;
using Application.Vacancies.Commands.Create;
using Application.Vacancies.Commands.Edit;
using Application.Vacancies.Dtos;
using Application.Vacancies.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VacanciesController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllVacancies()
        {
            var command = new GetVacancyTablesListQuery();
            return Ok(await Mediator.Send(command));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetVacancy(
            [FromRoute] string Id)
        {
            var query = new GetVacancyByIdQuery(Id);
            return StatusCode(201, await Mediator.Send(query));
        }
        [AllowAnonymous]
        [HttpGet("noauth/{Id}")]
        public async Task<IActionResult> GetVacancyNoAuth(
            [FromRoute] string Id)
        {
            var query = new GetVacancyByIdNoAuth(Id);
            return StatusCode(200, await Mediator.Send(query));
        }
        [HttpGet("short")]
        public async Task<IActionResult> GetAllShort()
        {
            var command = new GetShortVacanciesWithDepartmentQuery();
            return Ok(await Mediator.Send(command));
        }
        [HttpGet("applicant/{applicantId}")]
        public async Task<IActionResult> GetAllNotAppliedVacanciesByApplicant(string applicantId)
        {
            var command = new GetNotAppliedVacanciesByApplicantIdQuery(applicantId);
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> CreateVacancyWithStages(VacancyCreateDto vacancy)
        {
            var command = new CreateVacancyCommand(vacancy);
            return StatusCode(201, await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVacancy(VacancyUpdateDto vacancy, [FromRoute] string id)
        {
            var command = new EditVacancyCommand(vacancy, id);
            return StatusCode(201, await Mediator.Send(command));
        }
        [HttpPost("{vacancyId}/stages")]
        public async Task<IActionResult> CreateVacancyStage(StageCreateDto stage, [FromRoute] string vacancyId)
        {
            var command = new CreateVacancyStageCommand(stage, vacancyId);
            return StatusCode(201, await Mediator.Send(command));
        }
        [HttpPut("{vacancyId}/stages/{stageId}")]
        public async Task<IActionResult> EditVacancyStage(StageUpdateDto stage, [FromRoute] string vacancyId, [FromRoute] string stageId)
        {
            var command = new EditVacancyStageCommand(stage, vacancyId, stageId);
            return StatusCode(201, await Mediator.Send(command));
        }
        //[HttpDelete("{vacancyId}/stages/{stageId}")]
        //public async Task<IActionResult> DeleteVacancyStage(StageUpdateDto stage, [FromRoute] string vacancyId, [FromRoute] string stageId)
        //{
        //    var command = new DeleteVacancyStageCommand(stage, vacancyId, stageId);
        //    return StatusCode(201, await Mediator.Send(command));
        //}
    }
}
