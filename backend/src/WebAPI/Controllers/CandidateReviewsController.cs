using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.CandidateReviews.Dtos;
using Application.CandidateReviews.Commands;

namespace WebAPI.Controllers
{
    public class CandidateReviewsController : ApiController
    {
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkReviewDto data)
        {
            var command = new BulkReviewCommand(data);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
