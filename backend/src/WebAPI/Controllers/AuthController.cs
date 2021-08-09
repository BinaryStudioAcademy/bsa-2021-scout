using Application.Auth.Commands;
using Application.Auth.Dtos;
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
            try
            {
                var command = new LoginCommand(userLogin);
                return Ok(await Mediator.Send(command));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
