using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applicants.Dtos
{
    public class MarkedApplicantDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsApplied { get; set; }
    }
}
