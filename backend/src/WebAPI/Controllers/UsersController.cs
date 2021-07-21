using Application.Users.Commands.Create;
using Application.Users.Dtos;
using Application.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var query = new GetUserByIdQuery(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            var command = new CreateUserCommand(user);
            return StatusCode(201, await Mediator.Send(command));
        }
    }
}
