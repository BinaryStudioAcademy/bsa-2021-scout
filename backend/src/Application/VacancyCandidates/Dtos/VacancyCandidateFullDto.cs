using System;
using System.Collections.Generic;
using Application.CandidateReviews.Dtos;
using Application.Common.Models;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateFullDto : Dto
    {
        public string HrWhoAddedFullName { get; set; }
        public string CurrentStageName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Cv { get; set; }
        public double Experience { get; set; }
        public string Comments { get; set; }
        public IEnumerable<CandidateReviewShortDto> Reviews { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
