

namespace Application.Auth.Dtos
{
    public class ResetPasswordDto: ResetTokenDto
    {
        public string Password { get; set; }
    }
}
