using Application.Common.Models;

namespace Domain.Entities
{
    public class EmailTokenDto : Dto
    {
        public string UserId { get; set; }

        public string Token { get; set; }

    }
}
