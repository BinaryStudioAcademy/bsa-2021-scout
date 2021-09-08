using System.IO;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Application.Auth.Exceptions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected string GetUserIdFromToken()
        {
            string claimsUserId = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (string.IsNullOrEmpty(claimsUserId))
            {
                throw new InvalidTokenException("access");
            }

            return claimsUserId;
        }

        protected byte[] GetFileBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            MemoryStream memory = new MemoryStream();
            stream.CopyTo(memory);

            return memory.ToArray();
        }

        protected ActionResult ValidateFileType(IFormFile file, params string[] allowed)
        {
            foreach (string type in allowed)
            {
                if (file.ContentType == type)
                {
                    return null;
                }
            }

            return StatusCode(415, new
            {
                Message = $"Disallowed file type. Allowed: {string.Join(", ", allowed)}",
            });
        }
    }
}
