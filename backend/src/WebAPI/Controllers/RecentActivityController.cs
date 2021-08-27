using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Application.CandidateToStages.Dtos;
using Application.CandidateToStages.Queries;

namespace WebAPI.Controllers
{
    public class RecentActivityController : ApiController
    {
        [HttpGet("{page}")]
        public async Task<ActionResult<IEnumerable<CandidateToStageRecentActivityDto>>> GetPage([FromRoute] int page)
        {
            string userId = GetUserIdFromToken();
            return Ok(await Mediator.Send(new GetRecentActivityQuery(userId, page)));
        }
    }
}
