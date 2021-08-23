using Application.Applicants.Dtos;
using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Pools.Dtos
{
    public class PoolDto : Dto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }        
        public ICollection<PoolApplicantDto> Applicants { get; set; }

    }
}