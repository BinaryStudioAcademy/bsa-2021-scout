using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.VacancyCandidates.Dtos;
using Application.VacancyCandidates.Commands;

namespace WebAPI.Controllers
{
    public class VacancyCandidates : ApiController
    {
        [HttpPut("{id}/set-stage/{stageId}")]
        public async Task<ActionResult<VacancyCandidateDto>> ChangeCandidateStage(
            [FromRoute] string id,
            [FromRoute] string stageId
        )
        {
            var command = new ChangeCandidateStageCommand(id, stageId);
            return Ok(await Mediator.Send(command));
        }
    }
}
