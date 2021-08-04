using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Vacancies.Dtos;
using Application.Stages.Queries;

namespace WebAPI.Controllers
{
    public class StagesController : ApiController
    {
        [HttpGet("by-vacancy/{id}")]
        public async Task<ActionResult<ShortVacancyWithStagesDto>> GetByVacancyAsyncId([FromRoute] string id)
        {
            var query = new GetStagesByVacancyQuery(id);
            return Ok(await Mediator.Send(query));
        }
    }
}
