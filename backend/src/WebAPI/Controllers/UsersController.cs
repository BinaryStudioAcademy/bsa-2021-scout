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
using Application.Projects.Commands;
using Application.Users.Commands.Create;
using Microsoft.AspNetCore.Http;
using Application.Common.Files.Dtos;
using Newtonsoft.Json;
using Application.Users.Queries.GetUserById;

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
            var query = new GetUserByIdQuery(id);
            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetUsersForHrLeadQuery();
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetUsersForHrLeadQuery();
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

        [HttpGet("current/company/projects")]
        public async Task<IActionResult> CurrentHRProjects()
        {
            var query = new GetProjectsByCurrentHRCompanyCommand();
            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutUserAsync([FromForm] string body, [FromForm] IFormFile cvFile = null)
        {

            var updateDto = JsonConvert.DeserializeObject<UserUpdateDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var query = new UpdateUserCommand(updateDto!, cvFileDto);
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("pending-registrations")]
        public async Task<IActionResult> GetPendingRegistrations()
        {
            var query = new GetPendingRegistrationsQuery();
            return Ok(await Mediator.Send(query));
        }
    }
}
