using System;
using Application.Common.Models;
using System.Collections.Generic;
using Application.ElasticEnities.Dtos;

namespace Application.Applicants.Dtos
{
    public class ApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public double Experience { get; set; }
        public string ExperienceDescription { get; set; }
        public string Skills { get; set; }
        public bool HasCv { get; set; }
        public bool HasPhoto { get; set; }
        public bool IsSelfApplied { get; set; }
        public string PhotoLink { get; set; }
        public DateTime CreationDate { get; set; }

        public IEnumerable<ApplicantVacancyInfoDto> Vacancies { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }
}