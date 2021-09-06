using Application.Applicants.Dtos;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Tasks.Dtos
{
    public class TeamMemberstDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }

    }
}