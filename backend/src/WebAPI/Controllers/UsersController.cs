using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Extensions;
using Application.Interfaces;
using Application.Common.Exceptions;
using Domain.Entities;

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
        public async Task<ActionResult<UserDto>> GetUserFromToken([FromServices] ICurrentUserContext currentUserContext)
        {
            var user = await currentUserContext.GetCurrentUser();
            if (user is null)
            {
                return NotFound();
            }
            return Ok(user);
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
