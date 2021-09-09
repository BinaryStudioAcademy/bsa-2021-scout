using System.Collections.Generic;
using Domain.Enums;
using Application.VacancyCandidates.Dtos;
using Application.Reviews.Dtos;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Stages.Dtos
{
    public class StageWithCandidatesDto : Dto
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string VacancyId { get; set; }
        public string? DataJson { get; set; }
        public ICollection<Action> Actions { get; set; }
        public IEnumerable<ShortVacancyCandidateWithApplicantDto> Candidates { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
