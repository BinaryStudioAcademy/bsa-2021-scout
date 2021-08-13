using Application.Common.Models;

namespace Application.Auth.Dtos
{
    public class ConfirmEmailDto
    {
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
