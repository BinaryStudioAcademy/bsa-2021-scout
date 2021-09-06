using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.VacancyCandidates.Dtos;
using Application.VacancyCandidates.Queries;
using Application.VacancyCandidates.Commands;

namespace WebAPI.Controllers
{
    public class VacancyCandidates : ApiController
    {
        [HttpGet("{vacancyId}/{id}/full")]
        public async Task<ActionResult<VacancyCandidateFullDto>> GetFull(
            [FromRoute] string id,
            [FromRoute] string vacancyId
        )
        {
            var query = new GetFullVacancyCandidateByIdQuery(id, vacancyId);
            return Ok(await Mediator.Send(query));
        }

        [HttpPut("{id}/set-stage/{vacancyId}/{stageId}")]
        public async Task<ActionResult<VacancyCandidateDto>> ChangeCandidateStage(
            [FromRoute] string id,
            [FromRoute] string vacancyId,
            [FromRoute] string stageId
        )
        {
            string userId = GetUserIdFromToken();
            var command = new ChangeCandidateStageCommand(userId, id, vacancyId, stageId);

            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CandidatesRange/{vacancyId}")]
        public async Task<IActionResult> PostRangeOfCandidatesAsync(string[] applicantsIds, string vacancyId)
        {
            string userId = GetUserIdFromToken();
            var command = new CreateVacancyCandidateRangeCommand(applicantsIds, vacancyId, userId);

            return Ok(await Mediator.Send(command));
        }
        [HttpPost("{vacancyId}/{id}")]
        public async Task<IActionResult> PostVacancyCandidateNoAuth(string id, string vacancyId)
        {
            var command = new CreateVacancyCandidateNoAuthCommand(id, vacancyId);

            return Ok(await Mediator.Send(command));
        }
        [HttpPost("viewed/{id}")]
        public async Task<IActionResult> PostMarkAsViewed(string id)
        {
            var command = new MarkAsViewedCommand(id);

            return Ok(await Mediator.Send(command));
        }
    }
}
