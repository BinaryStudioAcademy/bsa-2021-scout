using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Dtos
{
    public class RegisterPermissionShortDto : Dto
    {
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
