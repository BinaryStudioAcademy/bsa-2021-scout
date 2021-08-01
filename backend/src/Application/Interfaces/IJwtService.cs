using Application.Users.Dtos;
using Domain.Common.Auth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateJsonWebToken(UserDto user);
        string GenerateRefreshToken();
        string GetUserIdFromToken(string accessToken, string signingKey);
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
