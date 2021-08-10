using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;

namespace WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ICvParser _cvParser;

        public UsersController(ICvParser cvParser)
        {
            _cvParser = cvParser;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var query = new GetEntityByIdQuery<UserDto>(id);
            await _cvParser.ParseAsync(@"
Hello, my name is Roman Melamud
My contacts:
  Email: 2m.roman2@gmail.com
  Phone: +380951550028
Organization: Binary Studio Corp.
My skills:
  Software Testing
  C#
  .NET
  Python Development
");
            return Ok(await Mediator.Send(query));
        }

        [AllowAnonymous]
        [HttpGet, Route("Email/{email}")]
        public async Task<IActionResult> IsEmailAlreadyUsed(string email)
        {
            var query = new IsEntityWithPropertyExistQuery("Email", email);
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
