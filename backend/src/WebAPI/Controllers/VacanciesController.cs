using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Common.Queries;
using Application.Vacancies.Commands.Create;
using Application.Vacancies.Commands.Edit;
using Application.Vacancies.Dtos;
using Application.Vacancies.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class VacanciesController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAllVacancies()
        {
            var command = new GetVacancyTablesListQuery();
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> CreateVacancy(VacancyCreateDto vacancy)
        {
            var command = new CreateVacancyCommand(vacancy);
            return StatusCode(201,await Mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditVacancy(VacancyUpdateDto vacancy, [FromRoute] string id)
        {
            var command = new EditVacancyCommand(vacancy, id);
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
