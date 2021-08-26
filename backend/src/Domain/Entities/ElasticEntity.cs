using System.Collections.Generic;
using System.Text.Json.Serialization;
using Domain.Common;
using System.Runtime.Serialization;
using Elasticsearch.Net;
using System;
using Newtonsoft.Json.Converters;
using Nest;

namespace Domain.Entities
{
    [Serializable]
    [StringEnum]
    public enum ElasticType
    {
        ApplicantTags = 1,
        VacancyTags = 2,
        ProjectTags = 3,
    }

    public class ElasticEntity : Entity
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [Text]
        public ElasticType ElasticType { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}