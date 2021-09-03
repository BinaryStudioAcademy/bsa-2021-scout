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
    [Authorize]
    [ApiController]
    public class RegisterController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RegisterDto registerDto)
        {
            var command = new RegisterUserCommand(registerDto);
            await Mediator.Send(command);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<ActionResult<AuthUserDto>> ComfirmEmail([FromBody] ConfirmEmailDto emailTokenDto)
        {
            var comfirmUserEmailCommand = new ComfirmUserEmailCommand(emailTokenDto);
            var authUserDto = await Mediator.Send(comfirmUserEmailCommand);
            return CreatedAtAction("GetUser", "users", new { id = authUserDto.User.Id }, authUserDto);
        }

        [AllowAnonymous]
        [HttpPost("resend-confirm-email")]
        public async Task<ActionResult<AuthUserDto>> Reseb([FromBody] ResendConfirmEmailDto resendConfirmEmailDto)
        {
            var command = new ResendConfirmEmailCommand(resendConfirmEmailDto);
            await Mediator.Send(command);
            return Ok();
        }

        [Authorize(Roles = "HrLead")]
        [HttpPost("send-registration-link")]
        public async Task<ActionResult<AuthUserDto>> SendRegistrationLink([FromBody] RegistrationLinkDto sendRegistrationLinkDto)
        {
            var command = new SendRegistrationLinkCommand(sendRegistrationLinkDto);
            await Mediator.Send(command);
            return Ok();
        }

        [Authorize(Roles = "HrLead")]
        [HttpPut("resend-registration-link")]
        public async Task<IActionResult> ResendRegistrationLink([FromBody] RegisterPermissionShortDto registerPermissionShortDto)
        {
            var command = new ResendRegistrationLinkCommand(registerPermissionShortDto);
            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "HrLead")]
        [HttpDelete("revoke-registration-link/{id}")]
        public async Task<IActionResult> RevokeRegistrationLink(string id)
        {
            var command = new RevokeRegistrationLinkCommand(id);
            return Ok(await Mediator.Send(command));
        }
    }
}
