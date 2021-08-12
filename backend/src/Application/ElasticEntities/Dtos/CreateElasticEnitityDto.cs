using System.Collections.Generic;
using Application.Common.Models;
using Domain.Entities;

namespace Application.ElasticEnities.Dtos
{
    public class CreateElasticEntityDto : Dto
    {
        public ElasticType ElasticType { get; set; }
        public IEnumerable<TagDto> TagsDtos { get; set; }
    }
}