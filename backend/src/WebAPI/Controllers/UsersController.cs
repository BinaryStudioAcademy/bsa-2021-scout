using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var query = new GetEntityByIdQuery<UserDto>(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("from-token")]
        public async Task<ActionResult<UserDto>> GetUserFromToken()
        {
            var query = new GetEntityByIdQuery<UserDto>(this.GetUserIdFromToken());
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
