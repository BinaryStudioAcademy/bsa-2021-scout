using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Common.Queries;
using Application.Stages.Commands;
using Application.Stages.Dtos;
using Application.UserFollowed.Commands;
using Application.UserFollowed.Dtos;
using Application.UserFollowed.Queries;
using Application.Vacancies.Commands.Create;
using Application.Vacancies.Commands.Edit;
using Application.Vacancies.Dtos;
using Application.Vacancies.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserFollowedController : ApiController
    {

        public UserFollowedController()
        {

        }
        [HttpGet("{type}")]
        public async Task<IActionResult> GetFollowedItemsByType([FromRoute] EntityType type)
        {
            var command = new GetUserFollowedEntityByType(type);
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> CreateFollowed(CreateUserFollowedDto followedDto)
        {
            var command = new CreateUserFollowedCommand(followedDto);
            return StatusCode(201, await Mediator.Send(command));
        }
        [HttpDelete("{id}/{type}")]
        public async Task<IActionResult> DeleteVacancy(string id, EntityType type)
        {
            var command = new DeleteUserFollowedCommand(id, type);
            return StatusCode(204, await Mediator.Send(command));
        }
    }
}
