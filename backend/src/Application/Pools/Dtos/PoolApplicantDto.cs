using Application.Applicants.Dtos;
using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Pools.Dtos
{
    public class PoolApplicantDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}