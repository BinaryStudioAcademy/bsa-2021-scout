using System;
using System.Collections.Generic;
using Application.ElasticEnities.Dtos;
using Application.CandidateReviews.Dtos;
using Application.CandidateToStages.Dtos;
using Application.Common.Models;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateFullDto : Dto
    {
        public string HrWhoAddedId { get; set; }
        public string HrWhoAddedFullName { get; set; }
        public string CurrentStageName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double Experience { get; set; }
        public string ExperienceDescription { get; set; }
        public string Comments { get; set; }
        public string CvLink { get; set; }
        public string CvName { get; set; }
        public string PhotoLink { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsSelfApplied { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
        public IEnumerable<CandidateToStageHistoryDto> StagesHistory { get; set; }
        public IEnumerable<CandidateReviewShortDto> Reviews { get; set; }
    }
}
