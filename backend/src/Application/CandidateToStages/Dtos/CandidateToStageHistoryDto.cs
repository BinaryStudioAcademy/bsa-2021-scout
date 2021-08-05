using System;
using Application.Common.Models;

namespace Application.CandidateToStages.Dtos
{
    public class CandidateToStageHistoryDto : Dto
    {
        public string StageName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateRemoved { get; set; }
    }
}
