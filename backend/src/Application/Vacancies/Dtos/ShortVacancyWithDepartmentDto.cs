using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Vacancies.Dtos
{
    public class ShortVacancyWithDepartmentDto : Dto
    {
        public string Title { get; set; }
        public string ProjectId { get; set; }
        public string Department { get; set; }
    }
}
