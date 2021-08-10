using System.Collections.Generic;
using Application.Common.Models;

namespace Application.ApplicantToTags.Dtos
{
    public class ApplicantToTagsDto: Dto
    {
        public IEnumerable<TagDto> TagDtos { get; set; }
    }
}