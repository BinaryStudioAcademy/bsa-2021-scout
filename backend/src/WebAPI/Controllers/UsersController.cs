using Application.Common.Commands;
using Application.Common.Queries;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using System.Collections.Generic;
using Application.Users.Queries;
using System;

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
        [HttpGet("check-connection-env")]
        public ActionResult<string> Check(){
            return Environment.GetEnvironmentVariable("ELASTIC_CONNECTION_STRING");
        }

        [AllowAnonymous]
        [HttpGet, Route("Email/{email}")]
        public async Task<IActionResult> IsEmailAlreadyUsed(string email)
        {
            var query = new IsEntityWithPropertyExistQuery("Email", email);
            return Ok(await Mediator.Send(query));
        }

        [Authorize(Roles = "HrLead")]
        [HttpGet("for-hr-lead")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUserForHrLead()
        {
            var query = new GetUsersForHrLeadQuery();
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
