﻿using Application.Auth.Commands;
using Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TokenController : ApiController
    {
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<AccessTokenDto>> Refresh([FromBody] RefreshTokenDto refreshToken)
        {
            var command = new RefreshTokenCommand(refreshToken);
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenDto revokeRefreshToken)
        {
            var userId = this.GetUserIdFromToken();
            var command = new LogoutCommand(revokeRefreshToken.RefreshToken, userId);
            await Mediator.Send(command);

            return Ok();
        }
    }
}