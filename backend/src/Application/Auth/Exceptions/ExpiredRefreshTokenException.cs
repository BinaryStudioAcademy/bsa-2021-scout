using Newtonsoft.Json;
using System;

namespace Application.Auth.Exceptions
{
    public class ExpiredRefreshTokenException : Exception
    {
        public ExpiredRefreshTokenException() :
            base(JsonConvert.SerializeObject(new
            {
                type = "ExpiredRefreshToken",
                description = "Refresh token expired"
            }))
        { }
    }
}
