using System;

namespace Application.Auth.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string tokenName) : base($"Invalid {tokenName} token.") { }
    }
}
