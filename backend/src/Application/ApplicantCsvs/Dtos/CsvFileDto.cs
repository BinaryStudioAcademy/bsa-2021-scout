using Application.Common.Models;
using Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ApplicantCsvs.Dtos
{
    public class CsvFileDto : Dto
    {
        public string Name { get; set; }
        public string Json { get; set; }
        public DateTime DateAdded { get; set; }
        public UserDto User { get; set; }
    }
}
