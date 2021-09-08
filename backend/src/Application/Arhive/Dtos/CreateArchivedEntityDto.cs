using Domain.Enums;

namespace Application.Arhive.Dtos
{
    public class CreateArchivedEntityDto
    {
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
    }
}
