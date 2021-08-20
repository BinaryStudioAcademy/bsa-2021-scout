using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Common.Queries;
using Application.Users.Commands;
using Application.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class RegisterController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RegisterDto registerDto)
        {
            var command = new RegisterUserCommand(registerDto);
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPost("confirm-email")]
        public async Task<ActionResult<AuthUserDto>> ComfirmEmail([FromBody] ConfirmEmailDto emailTokenDto)
        {
            var comfirmUserEmailCommand = new ComfirmUserEmailCommand(emailTokenDto);
            var authUserDto = await Mediator.Send(comfirmUserEmailCommand);
            return CreatedAtAction("GetUser", "users", new { id = authUserDto.User.Id }, authUserDto);
        }

        [HttpPost("resend-confirm-email")]
        public async Task<ActionResult<AuthUserDto>> Reseb([FromBody] ResendConfirmEmailDto resendConfirmEmailDto)
        {
            var command = new ResendConfirmEmailCommand(resendConfirmEmailDto);
            await Mediator.Send(command);
            return Ok();
        }
    }
}
