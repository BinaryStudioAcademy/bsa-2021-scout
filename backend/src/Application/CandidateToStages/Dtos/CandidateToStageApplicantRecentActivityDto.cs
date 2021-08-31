using System;
using Application.Common.Models;

namespace Application.CandidateToStages.Dtos
{
    public class CandidateToStageApplicantRecentActivityDto : Dto
    {
        public string MoverId { get; set; }
        public string MoverName { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
