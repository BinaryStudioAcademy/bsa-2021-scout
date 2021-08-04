using System.Collections.Generic;
using Domain.Enums;
using Application.VacancyCandidates.Dtos;

namespace Application.Stages.Dtos
{
    public class StageWithCandidatesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string VacancyId { get; set; }
        public ICollection<VacancyCandidateWithApplicantDto> Candidates { get; set; }
    }
}
