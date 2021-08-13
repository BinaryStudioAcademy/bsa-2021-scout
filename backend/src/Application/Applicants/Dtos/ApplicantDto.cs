using Application.Common.Models;
using System.Collections.Generic;
using Application.ElasticEnities.Dtos;

namespace Application.Applicants.Dtos
{
    public class ApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public IEnumerable<ApplicantVacancyInfoDto> Vacancies { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }
}