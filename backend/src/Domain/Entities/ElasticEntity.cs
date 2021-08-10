using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public enum ElasticType {
        ApplicantTags,
        VacancyTags
    }
    public class ElasticEntity: Entity
    {
        public ElasticType EnitityType { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}