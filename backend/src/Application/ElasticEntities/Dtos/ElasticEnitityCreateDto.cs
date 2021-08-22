using System.Collections.Generic;
using Application.Common.Models;
using Application.ElasticEntities.Dtos;
using Domain.Entities;

namespace Application.ElasticEnities.Dtos
{
    public class ElasticEnitityCreateDto
    {
        public ElasticType ElasticType { get; set; }
        public IEnumerable<TagsCreateDto> TagDtos { get; set; }
    }
}