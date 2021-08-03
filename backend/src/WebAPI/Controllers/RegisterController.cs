using Application.Auth.Dtos;
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
        public async Task<ActionResult<AuthUserDto>> Post([FromBody] UserRegisterDto user)
        {
            var command = new RegisterUserCommand(user);
            var createdUserWithToken = await Mediator.Send(command);

            return CreatedAtAction("GetUser", "users", new { id = createdUserWithToken.User.Id }, createdUserWithToken);
        }
    }
}
