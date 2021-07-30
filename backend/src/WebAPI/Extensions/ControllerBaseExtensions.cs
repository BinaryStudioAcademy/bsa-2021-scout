using Application.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebAPI.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static string GetUserIdFromToken(this ControllerBase controller)
        {
            var claimsUserId = controller.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (string.IsNullOrEmpty(claimsUserId))
            {
                throw new InvalidTokenException("access");
            }

            return claimsUserId;
        }
    }
}
