using System;
using Application.Common.Models;

namespace Application.ApplicantCv.Dtos
{
    public class ApplicantCvDto : MongoDto
    {
        public Guid ApplicantId { get; set; }
        public string Cv { get; set; }
    }
}
