using Domain.Common.Auth;
using Domain.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateJsonWebToken(User user);
        string GenerateRefreshToken();
        Guid GetUserIdFromToken(string accessToken, string signingKey);
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}
