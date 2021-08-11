using System.Collections.Generic;
using Application.Common.Models;
using Domain.Entities;

namespace Application.ElasticEnities.Dtos
{
    public class ElasticEnitityDto: Dto
    {
        public ElasticType ElasticType { get; set; }
        public IEnumerable<TagDto> TagDtos { get; set; }
    }
}