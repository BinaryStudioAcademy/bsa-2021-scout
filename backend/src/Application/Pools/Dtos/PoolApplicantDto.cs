using Application.Applicants.Dtos;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Pools.Dtos
{
    public class PoolApplicantDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoLink { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }
}