using Newtonsoft.Json;
using System;

namespace Application.Auth.Dtos
{
    public class RefreshTokenDto
    {
        public RefreshTokenDto()
        {
            SigningKey = Environment.GetEnvironmentVariable("SECRET_JWT_KEY");
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        [JsonIgnore]
        public string SigningKey { get; private set; }
    }
}
