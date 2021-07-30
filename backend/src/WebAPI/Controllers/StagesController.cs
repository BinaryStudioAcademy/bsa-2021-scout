using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Application.Stages.Dtos;
using Application.Stages.Queries;

namespace WebAPI.Controllers
{
    public class StagesController : ApiController
    {
        [HttpGet("by-vacancy/{id}")]
        public async Task<ActionResult<IEnumerable<StageWithCandidatesDto>>> GetByVacancyId([FromRoute] string id)
        {
            var query = new GetStagesByVacancyQuery(id);
            return Ok(await Mediator.Send(query));
        }
    }
}
