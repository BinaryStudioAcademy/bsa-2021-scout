using System;
using Application.Common.Models;

namespace Application.CandidateToStages.Dtos
{
    public class CandidateToStageRecentActivityDto : Dto
    {
        public string MoverId { get; set; }
        public string MoverName { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string VacancyId { get; set; }
        public string VacancyName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
