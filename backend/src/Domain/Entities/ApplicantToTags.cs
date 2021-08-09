using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class ApplicantToTags: Entity
    {
        public IEnumerable<Tag> Tags { get; set; }
    }
}