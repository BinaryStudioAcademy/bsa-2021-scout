using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Queries;
using Application.Reviews.Dtos;

namespace WebAPI.Controllers
{
    public class ReviewsController : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
        {
            var query = new GetEntityListQuery<ReviewDto>();
            return Ok(await Mediator.Send(query));
        }
    }
}
