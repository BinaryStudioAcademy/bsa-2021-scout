using System.Collections.Generic;
using Domain.Common;

namespace Application.Comprehend.Dtos
{
    public class ParsedEntitiesDto
    {
        public IEnumerable<TextEntity> Entities { get; set; }
    }
}
