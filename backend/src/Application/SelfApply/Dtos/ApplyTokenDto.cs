using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.SelfApply.Dtos
{
    public class ApplyTokenDto
    {
        public string Email { get; set; }

        public string VacancyId { get; set; }
        public string ClientUri { get; set; }
    }
}
