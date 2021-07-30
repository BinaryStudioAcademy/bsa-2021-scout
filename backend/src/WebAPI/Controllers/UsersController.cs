using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Application.Mail;
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
            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<ActionResult> Send()
        {
            var command = new SendMailCommand("2m.roman2@gmail.com", "Hello!", "My first email!");
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var command = new CreateEntityCommand<UserDto>(user);
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
