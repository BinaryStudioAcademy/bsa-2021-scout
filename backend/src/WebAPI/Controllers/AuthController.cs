using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Auth.Queries;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ApiController
    {
        [HttpPost("login")]
        public async Task<ActionResult<AuthUserDto>> Login(UserLoginDto userLogin)
        {
            var command = new LoginCommand(userLogin);
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var command = new ForgotPasswordCommand(forgotPasswordDto);
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var command = new ResetPasswordCommand(resetPasswordDto);
            return Ok(await Mediator.Send(command));
        }

        [HttpGet("reset-password")]
        public async Task<ActionResult<bool>> IsResetTokenValid([FromQuery] ResetTokenDto resetTokenDto)
        {
            var query = new IsResetTokenValidQuery(resetTokenDto);
            return Ok(await Mediator.Send(query));
        }
    }
}
