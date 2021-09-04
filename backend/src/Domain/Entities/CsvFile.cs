using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CsvFile : Entity
    {
        public string Name { get; set; }
        public string Json { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
    }

}
