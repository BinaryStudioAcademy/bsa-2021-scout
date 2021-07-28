using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Application.ApplicantCv.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var query = new GetEntityByIdQuery<UserDto>(id);
            await Mediator.Send(new CreateEntityCommand<ApplicantCvDto>(new ApplicantCvDto { ApplicantId = "9b2f4cd1-78f5-46bf-94ee-f2f2b18c2ce8", Cv = "abc" }));
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var command = new CreateEntityCommand<UserDto>(user);
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
