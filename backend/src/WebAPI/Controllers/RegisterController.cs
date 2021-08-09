using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Common.Queries;
using Application.Users.Commands;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class RegisterController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserRegisterDto user)
        {
            try
            {
                var command = new RegisterUserCommand(user);
                await Mediator.Send(command);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<AuthUserDto>> ComfirmEmail(string email, string token)
        {
            try
            {
                var comfirmUserEmailCommand = new ComfirmUserEmailCommand(email, token);
                var authUserDto = await Mediator.Send(comfirmUserEmailCommand);
                return CreatedAtAction("GetUser", "users", new { id = authUserDto.User.Id }, authUserDto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
