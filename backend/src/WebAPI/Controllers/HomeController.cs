using Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Home.Dtos;
using Application.Home.Queries;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class HomeController : ApiController
    {
        [HttpGet("widgets-data")]
        public async Task<ActionResult<WidgetsDataDto>> GetWidgetsData()
        {
            return Ok(await Mediator.Send(new GetWidgetsDataQuery()));
        }

        [HttpGet("vacancies")]
        public async Task<ActionResult<IEnumerable<HotVacancySummaryDto>>> GetHotVacancySummary()
        {
            return Ok(await Mediator.Send(new GetHotVacancySummaryQuery()));
        }
    }
}
