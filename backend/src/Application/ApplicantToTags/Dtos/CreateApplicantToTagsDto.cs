using System.Collections.Generic;
using Application.Common.Models;

namespace Application.ApplicantToTags.Dtos
{
    public class CreateApplicantToTagsDto : Dto
    {
        public IEnumerable<TagDto> TagsDtos { get; set; }
    }
}