using Newtonsoft.Json;
using System;

namespace Application.Auth.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string tokenName) :
            base(JsonConvert.SerializeObject(new
            {
                type = "InvalidToken",
                description = $"Invalid {tokenName} token"
            }))
        { }
    }
}
