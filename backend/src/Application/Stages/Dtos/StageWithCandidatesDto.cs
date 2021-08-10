using System.Collections.Generic;
using Domain.Enums;
using Application.VacancyCandidates.Dtos;
using Application.Common.Models;

namespace Application.Stages.Dtos
{
    public class StageWithCandidatesDto : Dto
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string VacancyId { get; set; }
        public IEnumerable<ShortVacancyCandidateWithApplicantDto> Candidates { get; set; }
    }
}
