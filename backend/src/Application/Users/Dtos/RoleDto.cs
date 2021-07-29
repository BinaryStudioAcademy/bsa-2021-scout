using Application.Common.Models;

namespace Application.Users.Dtos
{
    public class RoleDto: Dto
    {
        public string Name { get; set; }
        public int Key { get; set; }
    }
}
